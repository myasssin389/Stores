namespace Stores.Models;

public class Cart
{
    public int Id { get; set; }
    public ApplicationUser User { get; set; }
    public string UserId { get; set; }
    public ICollection<CartProductMap> CartProductMaps { get; set; }

    public decimal getTotalAmount()
    {
        decimal totalAmount = 0;
        foreach (var cartProduct in CartProductMaps)
        {
            totalAmount += cartProduct.Quantity * cartProduct.Product.Price;
        }
        return totalAmount;
    }
}