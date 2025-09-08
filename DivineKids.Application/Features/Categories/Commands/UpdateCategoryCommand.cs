namespace DivineKids.Application.Features.Categories.Commands;

public sealed class UpdateCategoryCommand
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public int MainCategoryId { get; init; }
}
