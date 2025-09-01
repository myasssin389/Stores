using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Stores.Data;
using Stores.Models;

namespace Stores.Controllers;

public class CartController : Controller
{
    private readonly StoresDbContext _context;

    public CartController(StoresDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public IActionResult AddToCart(int cartId, int productId, int quantity)
    {
        var map = new CartProductMap
        {
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity
        };
        
        _context.CartProductMaps.Add(map);
        _context.SaveChanges();
        
        return RedirectToAction("Index", "Home");
    }
}