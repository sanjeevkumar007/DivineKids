namespace DivineKids.Domain.Entities;

public sealed class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;

    public DateTimeOffset CreatedDate { get; private set; }
    public DateTimeOffset ModifiedDate { get; private set; }

    public bool RequiresShipping { get; private set; } = true;
    public decimal? WeightKg { get; private set; }
    public decimal? LengthCm { get; private set; }
    public decimal? WidthCm { get; private set; }
    public decimal? HeightCm { get; private set; }

    public int CategoryId { get; private set; }

    private Product() { }

    public Product(string name, decimal price, int categoryId, decimal weight, decimal? lengthCm, decimal widthCm, decimal? heightCm, string? description = null, string? imageUrl = null)
    {
        SetName(name);
        SetPrice(price);
        SetCategory(categoryId);
        SetDescription(description);
        SetImageUrl(imageUrl);
        SetWeightKg(weight);
        SetDimensions(lengthCm, widthCm, heightCm);


        var now = DateTimeOffset.UtcNow;
        CreatedDate = now;
        ModifiedDate = now;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        Name = name.Trim();
        Touch();
    }

    public void SetDescription(string? description)
    {
        Description = description?.Trim() ?? string.Empty;
        Touch();
    }

    public void SetPrice(decimal price)
    {
        if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price), "Price must be positive.");
        Price = price;
        Touch();
    }

    public void SetImageUrl(string? imageUrl)
    {
        ImageUrl = imageUrl?.Trim() ?? string.Empty;
        Touch();
    }

    public void SetCategory(int categoryId)
    {
        if (categoryId <= 0) throw new ArgumentOutOfRangeException(nameof(categoryId));
        CategoryId = categoryId;
        Touch();
    }

    public void SetRequiresShipping(bool requires)
    {
        RequiresShipping = requires;
        Touch();
    }

    public void SetWeightKg(decimal? weightKg)
    {
        if (weightKg is < 0) throw new ArgumentOutOfRangeException(nameof(weightKg));
        WeightKg = weightKg;
        Touch();
    }

    public void SetDimensions(decimal? lengthCm, decimal? widthCm, decimal? heightCm)
    {
        if (lengthCm is < 0 || widthCm is < 0 || heightCm is < 0) throw new ArgumentOutOfRangeException("Dimensions must be non-negative.");
        LengthCm = lengthCm;
        WidthCm = widthCm;
        HeightCm = heightCm;
        Touch();
    }


    private void Touch() => ModifiedDate = DateTimeOffset.UtcNow;
}