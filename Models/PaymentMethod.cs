using System.ComponentModel.DataAnnotations;

namespace Stores.Models;

public class PaymentMethod
{
    public int Id { get; set; }
    [MaxLength(30)]
    public string Name { get; set; }
}