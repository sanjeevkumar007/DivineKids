namespace DivineKids.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; private set; }
    public DateTimeOffset ModifiedDate { get; private set; }
    public int MainCategoryId { get; private set; }

    private Category() { }

    public Category(string name, int mainCategoryId, string? description = null, string? imageUrl = null)
    {
        SetName(name);
        SetMainCategory(mainCategoryId);
        SetDescription(description);
        SetImageUrl(imageUrl);

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

    public void SetImageUrl(string? imageUrl)
    {
        ImageUrl = imageUrl?.Trim() ?? string.Empty;
        Touch();
    }

    public void SetMainCategory(int mainCategoryId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(mainCategoryId);
        MainCategoryId = mainCategoryId;
        Touch();
    }

    private void Touch() => ModifiedDate = DateTimeOffset.UtcNow;
}