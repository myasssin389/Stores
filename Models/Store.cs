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
    
    [Required]
    [MaxLength(11)]
    public string? Phone { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Email { get; set; }
    
    [Required]
    public string StoreAdminId { get; set; }
    
    public ApplicationUser? StoreAdmin { get; set; }
    
    public Category Category { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    
    [Required]
    [MaxLength(18)]
    public string TaxRegistrationNumber { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string CommercialRegistrationNumber { get; set; }
    
    public ICollection<Product> Products { get; set; }
}