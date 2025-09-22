using DivineKids.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DivineKids.Infrastructure.Persistence.DesignTime;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Determine environment (falls back to Development)
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        // Build configuration (allows ConnectionStrings:Default override via env vars)
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Fallback connection string if none provided in config
        var connectionString = configuration.GetConnectionString("AzureMySqlConnection")
            ?? "Server=localhost;Port=3306;Database=DivineKidsDb;User Id=appuser;Password=ChangeMe123!;TreatTinyAsBoolean=true;";

        // Auto-detect server version (works for MariaDB/MySQL)
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseMySql(
            connectionString,
            serverVersion);

        return new AppDbContext(optionsBuilder.Options);
    }
}