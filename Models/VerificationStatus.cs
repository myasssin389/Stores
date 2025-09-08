using System.ComponentModel.DataAnnotations;

namespace Stores.Models;

public class VerificationStatus
{
    public byte Id { get; set; }
    [Required]
    [MaxLength(25)]
    public string Name { get; set; }
    [MaxLength(255)]
    public string? Feedback { get; set; }
}