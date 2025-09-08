namespace DivineKids.Application.Features.Categories.Commands;

public sealed class CreateCategoryCommand
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public int MainCategoryId { get; init; }
}
