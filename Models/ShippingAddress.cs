using System.ComponentModel.DataAnnotations;

namespace Stores.Models;

public class Address
{
    public int Id { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string City { get; set; }
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string StreetName { get; set; }
    [StringLength(10, MinimumLength = 1)]
    public string StreetNumber { get; set; }
    [Required]
    [StringLength(10, MinimumLength = 1)]
    public string BuildingNumber { get; set; }
    [Required]
    [StringLength(10, MinimumLength = 1)]
    public string ApartmentNumber { get; set; }
    [StringLength(10)]
    public string? ZipCode { get; set; }
}