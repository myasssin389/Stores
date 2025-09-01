using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stores.Models;

public class Product
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    public Store Store { get; set; }
    
    [Required]
    public int StoreId { get; set; }
    
    [Column(TypeName = "text")]
    public string? PhotoLink { get; set; }
    
}