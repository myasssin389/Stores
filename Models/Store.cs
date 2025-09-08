using System.ComponentModel.DataAnnotations;

namespace Stores.Models;

public class Store
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Address { get; set; }
    
    [MaxLength(100)]
    public string? City { get; set; }
    
    [MaxLength(11)]
    public string? Phone { get; set; }
    
    public Category Category { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    
    public byte VerificationStatusId { get; set; }
    
    [Required]
    public VerificationStatus VerificationStatus { get; set; }
    
    public ICollection<Product> Products { get; set; }
}