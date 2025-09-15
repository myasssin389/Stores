using Stores.Models;

namespace Stores.ViewModels;

public class UpdateOrderStatusViewModel
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public int StoreId { get; set; }
}