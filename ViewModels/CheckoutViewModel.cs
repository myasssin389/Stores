using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Stores.Models;

namespace Stores.ViewModels;

public class CheckoutViewModel
{
    [ValidateNever]
    public Cart Cart { get; set; }

    public ShippingAddress ShippingAddress { get; set; } = new ShippingAddress();

    [Required]
    public int SelectedPaymentMethodId { get; set; }
    
    [ValidateNever]
    public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
}