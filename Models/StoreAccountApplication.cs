using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stores.Models;

public class StoreAccountApplication
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string StoreName { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string StoreAddress { get; set; }
    
    [MaxLength(100)]
    public string? StoreCity { get; set; }
    
    [Required]
    [MaxLength(11)]
    public string? StorePhone { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string StoreEmail { get; set; }
    
    [Required]
    public string StoreAdminId { get; set; }
    
    public ApplicationUser? StoreAdmin { get; set; }
    
    public Category StoreCategory { get; set; }
    
    [Required]
    public int StoreCategoryId { get; set; }
    
    [Required]
    public byte VerificationStatusId { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    
    [Required]
    [MaxLength(18)]
    public string StoreTaxRegistrationNumber { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string StoreCommercialRegistrationNumber { get; set; }

    [Column(TypeName = "Text")]
    public string? Feedback { get; set; }
    
    [Required]
    public DateTime AppliedAt { get; set; }
}