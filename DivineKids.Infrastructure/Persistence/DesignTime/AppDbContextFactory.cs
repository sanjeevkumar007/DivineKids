using DivineKids.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DivineKids.Infrastructure.Persistence.DesignTime;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer("Server=H-5CH2271N89\\SQLEXPRESS01;Database=DivineKidsdDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");
        return new AppDbContext(builder.Options);
    }
}