namespace DivineKids.Application.Features.Catalog;

public sealed class CategoryNode
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public List<ProductLeaf> Products { get; init; } = new();
}