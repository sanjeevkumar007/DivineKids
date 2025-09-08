
namespace DivineKids.Domain.Entities;
public sealed class CartItem : BaseEntity
{
    public int CartId { get; private set; }
    public int ProductId { get; private set; }
    public decimal Quainty { get; private set; }

    public CartItem() { }

    public CartItem(int cartId, int productId, decimal quaintity)
    {
        SetCartId(cartId);
        SetProductId(productId);
        SetQuantity(quaintity);
    }

    private void SetQuantity(decimal quaintity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quaintity);
        Quainty = quaintity;
    }

    private void SetProductId(int productId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId);
        ProductId = productId;
    }

    private void SetCartId(int cartId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cartId);
        CartId = cartId;
    }
}
