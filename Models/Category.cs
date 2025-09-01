using System.ComponentModel.DataAnnotations;

namespace Stores.Models;

public class Category
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; }
    
    public ICollection<Store> Stores { get; set; }
}