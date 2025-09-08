namespace DivineKids.Application.Features.Dtos;

public sealed record ProductDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public DateTimeOffset CreatedDate { get; init; }
    public DateTimeOffset ModifiedDate { get; init; }
    public bool RequiresShipping { get; init; }
    public decimal? WeightKg { get; init; }
    public decimal? LengthCm { get; init; }
    public decimal? WidthCm { get; init; }
    public decimal? HeightCm { get; init; }
    public int CategoryId { get; init; }
}
