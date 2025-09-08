namespace DivineKids.Application.Features.Prodoucts.Commands;
public sealed class CreateProductCommand
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public bool RequiresShipping { get; init; } = true;
    public decimal? WeightKg { get; init; }
    public decimal? LengthCm { get; init; }
    public decimal? WidthCm { get; init; }
    public decimal? HeightCm { get; init; }
    public int CategoryId { get; init; }
}
