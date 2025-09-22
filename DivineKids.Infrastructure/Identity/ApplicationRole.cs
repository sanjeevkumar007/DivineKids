using Microsoft.AspNetCore.Identity;

namespace DivineKids.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<int>
{
    // Human-friendly label (can differ from Name which is used internally)
    public string? DisplayName { get; set; }

    // Long form description for admin UI
    public string? Description { get; set; }

    // True for roles the system depends on (e.g., Admin) -> block delete/update
    public bool IsSystemRole { get; set; } = true;

    // True if newly registered users should automatically receive it
    public bool IsDefaultForNewUsers { get; set; }

    // Audit
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; set; }
}