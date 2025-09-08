using DivineKids.Application.Features.Catalog;

public sealed class MainCategoryNode
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public List<CategoryNode> Categories { get; init; } = new();
}