using System.ComponentModel.DataAnnotations;

namespace Stores.Models;

public class OrderStatus
{
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }
}