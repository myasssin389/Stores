namespace Stores.ViewModels;

public class StoreViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProductViewModel> Products { get; set; } = new();
}