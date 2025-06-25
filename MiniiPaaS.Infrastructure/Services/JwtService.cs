using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniiPaaS.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config) => _config = config;

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()), // Enum'ı stringe çevir
            new Claim("CompanyId", user.CompanyId.ToString()) // Yeni: Şirket ID'si
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Token'dan CompanyId çözümlemek için ek metod (Opsiyonel)
    public int GetCompanyIdFromToken(ClaimsPrincipal user)
    {
        var companyIdClaim = user.FindFirst("CompanyId")?.Value;
        return int.TryParse(companyIdClaim, out var id) ? id : 0;
    }
}