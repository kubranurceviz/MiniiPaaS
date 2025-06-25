using BCrypt.Net; 
using Microsoft.EntityFrameworkCore;
using MiniiPaaS.Domain.Entities;
using MiniiPaaS.Domain.Enums;
using MiniiPaaS.Infrastructure.Data;
using Org.BouncyCastle.Crypto.Generators;

namespace MiniiPaaS.Infrastructure.Data
{
    public static class SeedData // Sınıf tanımı eklendi
    {
        public static void Initialize(MiniiPaaSDbContext context) // Metot ismi daha açıklayıcı yapıldı
        {
            if (!context.Users.Any(u => u.Role == Role.SuperAdmin))
            {
                var company = new Company { Id = 1, Name = "MiniPaaS Inc" }; // Önce şirket oluşturuldu
                context.Companies.Add(company);

                var superAdmin = new User
                {
                    Email = "superadmin@miniipaas.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                    Role = Role.SuperAdmin,
                    CompanyId = company.Id // Şirket ilişkisi
                };

                context.Users.Add(superAdmin);
                context.SaveChanges();
            }
        }
    }
}