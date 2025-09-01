using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Stores.Models;

namespace Stores.ViewModels;

public class StoreFormViewModel
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    public string City { get; set; }
    
    public string Phone { get; set; }
    
    [Required]
    public int Category { get; set; }
    
    [ValidateNever]
    public IEnumerable<Category> Categories { get; set; }
}