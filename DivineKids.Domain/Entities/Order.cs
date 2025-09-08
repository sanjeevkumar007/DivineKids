using DivineKids.Domain.Enums;

namespace DivineKids.Domain.Entities;
public class Order : BaseEntity
{
    public int UserId { get; private set; }
    public int AddressId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public Status StatusId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public Order() { }

    public Order(int userId, int addressId, decimal totalAmount, Status statusId)
    {
        SetUserId(userId);
        SetAddressId(addressId);
        SetTotalAmount(totalAmount);
        SetStatusId(statusId);

        var now = DateTimeOffset.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }

    private void SetStatusId(Status statusId)
    {
        ArgumentOutOfRangeException.ThrowIfNullOrEmpty(nameof(statusId));
        StatusId = statusId;
        Touch();
    }

    private void SetTotalAmount(decimal totalAmount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(totalAmount);
        TotalAmount = totalAmount;
        Touch();
    }

    private void SetAddressId(int addressId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(addressId);
        AddressId = addressId;
        Touch();
    }

    private void SetUserId(int userId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        UserId = userId;
        Touch();
    }

    private void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}
