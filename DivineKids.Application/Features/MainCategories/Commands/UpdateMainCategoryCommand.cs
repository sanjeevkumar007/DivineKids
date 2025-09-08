namespace DivineKids.Application.Features.MainCategories.Commands;
public sealed class UpdateMainCategoryCommand
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
}
