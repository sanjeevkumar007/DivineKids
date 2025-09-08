namespace DivineKids.Application.Features.Catalog;
public sealed class ProductLeaf
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
}