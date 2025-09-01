using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Stores.Models;

namespace Stores.ViewModels;

public class ProductFormViewModel
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string PhotoLink { get; set; }
    [Required]
    public int Store { get; set; }
    [ValidateNever]
    public IEnumerable<Store> Stores { get; set; }
}