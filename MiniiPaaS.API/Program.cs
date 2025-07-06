using System.Security.Claims;
using System.Text;
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
using MiniiPaaS.API.Middleware;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog konfigürasyonu
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: Serilog.RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Program.cs içinde builder.Services.Add... kısmına ekleyin
builder.Services.AddScoped<IEmailService, EmailService>();
// DbContext ve servisler
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

// --- Uygulamayı başlat ---
var app = builder.Build();

// Serilog HTTP Request Logging
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);

        if (httpContext.User.Identity?.IsAuthenticated ?? false)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var companyId = httpContext.User.FindFirst("CompanyId")?.Value;
            diagnosticContext.Set("UserId", userId);
            diagnosticContext.Set("CompanyId", companyId);
        }
    };
});

// Geliştirme ortamında Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 👇 Buraya özel logging middleware'in eklendi
app.UseMiddleware<LoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
