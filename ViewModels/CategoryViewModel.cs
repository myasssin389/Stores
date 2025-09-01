namespace Stores.ViewModels;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<StoreViewModel> Stores { get; set; } = new();
}