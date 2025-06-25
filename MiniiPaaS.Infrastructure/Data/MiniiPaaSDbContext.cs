// MiniiPaaS.Infrastructure/Data/MiniIPaaSDbContext.cs
using Microsoft.EntityFrameworkCore;
using MiniiPaaS.Domain.Entities;
using MiniiPaaS.Application.Interfaces;

namespace MiniiPaaS.Infrastructure.Data
{
    public class MiniiPaaSDbContext : DbContext, IApplicationDbContext
    {
        public MiniiPaaSDbContext(DbContextOptions<MiniiPaaSDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}