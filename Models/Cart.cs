namespace Stores.Models;

public class Cart
{
    public int Id { get; set; }
    public ApplicationUser User { get; set; }
    public string UserId { get; set; }
}