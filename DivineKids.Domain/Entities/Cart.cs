namespace DivineKids.Domain.Entities;
public sealed class Cart : BaseEntity
{
    public int UserId { get; private set; }
    public DateTimeOffset CreatedDate { get; private set; }
    public DateTimeOffset ModifiedDate { get; private set; }

    public Cart() { }

    public Cart(int userId)
    {
        SetUserId(userId);
        var now = DateTimeOffset.UtcNow;
        CreatedDate = now;
        ModifiedDate = now;
    }

    private void SetUserId(int userId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        UserId = userId;
        Touch();
    }

    private void Touch() => ModifiedDate = DateTimeOffset.UtcNow;
}
