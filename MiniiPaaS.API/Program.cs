using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniiPaaS.Application.Commands.UserManagement;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Domain.Entities;
using MiniiPaaS.Domain.Enums;
using MiniiPaaS.Infrastructure.Data;
using MiniiPaaS.Infrastructure.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Services ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext ve Interface Enjeksiyonu
builder.Services.AddDbContext<MiniiPaaSDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 41)),
        mysqlOptions =>
        {
            mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            mysqlOptions.EnableStringComparisonTranslations(true);
        }
    ));
builder.Services.AddScoped<IApplicationDbContext, MiniiPaaSDbContext>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
        };
    });

// Yetkilendirme Politikaları
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole(Role.SuperAdmin.ToString()));
    options.AddPolicy("CompanyAdminOnly", policy => policy.RequireRole(Role.CompanyAdmin.ToString()));
    options.AddPolicy("SameCompany", policy =>
        policy.RequireClaim("CompanyId")); 
});

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
});

// --- Build App ---
var app = builder.Build();

// --- Middleware Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();