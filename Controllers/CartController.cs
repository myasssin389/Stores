using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stores.Data;
using Stores.Models;
using Stores.ViewModels;

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

    [HttpPost("add/{productId}")]
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

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return NotFound("Product not found.");
        }
        if (product.Quantity < quantity)
        {
            return BadRequest("Not enough stock available.");
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

        product.Quantity -= quantity;
        
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost("remove/{productId}")]
    public async Task<IActionResult> DeleteFromCart(int productId)
    {
        var userId = _userManager.GetUserId(User);
        
        var cart = await _context.Carts
            .Include(c => c.CartProductMaps)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        
        if (cart == null)
        {
            return RedirectToAction("Index", "Home");
        }
        
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return NotFound("Product not found.");
        }
        
        var cartItem = cart.CartProductMaps.FirstOrDefault(c => c.ProductId == productId);
        if (cartItem == null)
        {
            return BadRequest("Item not found in cart.");
        }
        
        product.Quantity += cartItem.Quantity;
        cart.CartProductMaps.Remove(cartItem);
        await _context.SaveChangesAsync();
        
        var referer = Request.Headers["Referer"].ToString();
        return !string.IsNullOrEmpty(referer) 
            ? Redirect(referer) 
            : RedirectToAction("Checkout");
    }
    
    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var userId = _userManager.GetUserId(User);
        var cart = await _context.Carts
            .Include(c => c.CartProductMaps)
            .ThenInclude(cp => cp.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.CartProductMaps.Any())
            return RedirectToAction("Index", "Home");
        
        var paymentMethods = await _context.PaymentMethods.ToListAsync();

        var checkoutViewModel = new CheckoutViewModel
        {
            Cart = cart,
            PaymentMethods = paymentMethods
        };

        return View(checkoutViewModel);
    }
}