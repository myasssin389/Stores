using System.ComponentModel.DataAnnotations;

namespace Stores.Models;

public enum PaymentMethod
{
    [Display(Name = "Credit Card")]
    CreditCard = 1,
    
    [Display(Name = "Debit Card")]
    DebitCard = 2,
    
    [Display(Name = "Cash On Delivery")]
    CashOnDelivery = 3
}