// MiniiPaaS.Application/Interfaces/IApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using MiniiPaaS.Domain.Entities;

namespace MiniiPaaS.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Company> Companies { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}