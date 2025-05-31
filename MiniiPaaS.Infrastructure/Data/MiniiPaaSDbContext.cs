using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniiPaaS.Domain.Entities;
namespace MiniiPaaS.Infrastructure.Data
{
    public class MiniIPaaSDbContext : DbContext
    {
        public MiniIPaaSDbContext(DbContextOptions<MiniIPaaSDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfigürasyonlar buraya gelecek
        }
    }
}
