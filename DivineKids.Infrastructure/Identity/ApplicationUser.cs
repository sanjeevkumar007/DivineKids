using Microsoft.AspNetCore.Identity;

namespace DivineKids.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public required string Name { get; set; }
}