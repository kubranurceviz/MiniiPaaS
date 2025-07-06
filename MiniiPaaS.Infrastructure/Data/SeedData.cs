using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using MiniiPaaS.Domain.Entities;
using MiniiPaaS.Domain.Enums;

namespace MiniiPaaS.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(MiniiPaaSDbContext context)
        {
            // Eğer zaten bir SuperAdmin varsa tekrar ekleme
            if (!context.Users.Any(u => u.Role == Role.SuperAdmin))
            {
                // Eğer daha önce aynı Company yoksa ekle
                var existingCompany = context.Companies.FirstOrDefault(c => c.Id == 1);
                if (existingCompany == null)
                {
                    existingCompany = new Company { Id = 1, Name = "MiniPaaS Inc" };
                    context.Companies.Add(existingCompany);
                }

                var superAdmin = new User
                {
                    Email = "superadmin@minipaas.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("SuperAdmin123!"),
                    Role = Role.SuperAdmin,
                    CompanyId = existingCompany.Id,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                context.Users.Add(superAdmin);
                context.SaveChanges();
            }
        }
    }
}
