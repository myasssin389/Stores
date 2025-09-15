using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stores.Models;

public class Order
{
    public int Id { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public DateTime? DeliveryDate { get; set; }
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }
    [Required]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    [Required]
    public int ShippingAddressId { get; set; }
    public Address ShippingAddress { get; set; }
    public int? BillingAddressId { get; set; }
    public Address? BillingAddress { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    [Required]
    public int PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public OrderStatus Status { get; set; }
    
    [Required]
    public int StoreId { get; set; }
    
    public Store? Store { get; set; }
}