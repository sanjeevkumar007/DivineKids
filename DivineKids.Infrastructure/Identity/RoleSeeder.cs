using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DivineKids.Infrastructure.Identity;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(this IServiceProvider services, CancellationToken ct = default)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        var definitions = new[]
        {
            new ApplicationRole
            {
                Name = RoleNames.Admin,
                NormalizedName = RoleNames.Admin.ToUpperInvariant(),
                DisplayName = "Administrator",
                Description = "Full administrative access",
                IsSystemRole = true
            },
            new ApplicationRole
            {
                Name = RoleNames.User,
                NormalizedName = RoleNames.User.ToUpperInvariant(),
                DisplayName = "Customer",
                Description = "Standard authenticated user",
                IsDefaultForNewUsers = true
            }
        };

        foreach (var def in definitions)
        {
            if (await roleManager.RoleExistsAsync(def.Name!)) continue;
            var result = await roleManager.CreateAsync(def);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}:{e.Description}"));
                throw new InvalidOperationException($"Failed to create role {def.Name}: {errors}");
            }
        }
    }
}