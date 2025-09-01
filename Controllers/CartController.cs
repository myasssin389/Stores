using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stores.Data;
using Stores.Models;

namespace Stores.Controllers;

[Authorize]
[Route("api/cart")]
public class CartController : Controller
{
    private readonly StoresDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(StoresDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        var userId = _userManager.GetUserId(User);

        var cart = await _context.Carts
            .Include(c => c.CartProductMaps)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var cartItem = cart.CartProductMaps.FirstOrDefault(c => c.ProductId == productId);
        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cart.CartProductMaps.Add(new CartProductMap
            {
                ProductId = productId,
                Quantity = quantity
            });
        }

        await _context.SaveChangesAsync();
        
        return Redirect(Request.Headers["Referer"].ToString());
    }
}