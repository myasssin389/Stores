using System.ComponentModel.DataAnnotations;

namespace Stores.Models;

public enum OrderStatus
{
    Pending = 1,
    Confirmed = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}