/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace MiniiPaaS.Infrastructure.Data
{
    public class MiniIPaaSDbContextFactory : IDesignTimeDbContextFactory<MiniIPaaSDbContext>
    {
        public MiniIPaaSDbContext CreateDbContext(string[] args)
        {
            // appsettings.json dosyasını API projesinden al
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), ".." , "..", "MiniiPaaS.API");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<MiniIPaaSDbContext>();
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 41))); 

            return new MiniIPaaSDbContext(optionsBuilder.Options);
        }
    }
}*/
// MiniiPaaS.Infrastructure/Data/MiniIPaaSDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MiniiPaaS.Infrastructure.Data;

public class MiniIPaaSDbContextFactory : IDesignTimeDbContextFactory<MiniiPaaSDbContext>
{
    public MiniiPaaSDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<MiniiPaaSDbContext>();
        optionsBuilder.UseMySql(
            configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 41)));

        return new MiniiPaaSDbContext(optionsBuilder.Options);
    }
}