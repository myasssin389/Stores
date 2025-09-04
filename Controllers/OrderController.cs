using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Stores.Data;
using Stores.Models;
using Stores.ViewModels;

namespace Stores.Controllers;

public class OrderController : Controller
{
    private readonly StoresDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(StoresDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
    {
        var userId = _userManager.GetUserId(User);
        
        var cart = await _context.Carts
            .Include(c => c.CartProductMaps)
            .ThenInclude(c => c.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        
        if (cart == null || !cart.CartProductMaps.Any())
        {
            return RedirectToAction("Index", "Home");
        }
        
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        foreach (var error in errors)
        {
            Console.WriteLine(error.ErrorMessage);
        }

        
        if (!ModelState.IsValid)
        {
            model.PaymentMethods = await _context.PaymentMethods.ToListAsync();
            model.Cart = cart;
            return View("~/Views/Cart/Checkout.cshtml", model);
        }
        
        _context.ShippingAddresses.Add(model.ShippingAddress);

        var order = new Order
        {
            UserId = userId,
            ShippingAddress = model.ShippingAddress,
            PaymentMethodId = model.SelectedPaymentMethodId,
            Date = DateTime.Now,
            OrderItems = cart.CartProductMaps.Select(cp => new OrderItem
            {
                ProductId = cp.ProductId,
                Quantity = cp.Quantity
            }).ToList(),
            StatusId = 1,
            Total = cart.getTotalAmount()
        };

        
        _context.Orders.Add(order);
        
        _context.CartProductMaps.RemoveRange(cart.CartProductMaps);
        
        await _context.SaveChangesAsync();
        
        return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
    }

    public async Task<IActionResult> OrderConfirmation(int? orderId)
    {
        var userId = _userManager.GetUserId(User);
        
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.ShippingAddress)
            .Include(o => o.PaymentMethod)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        
        if (order == null)
            return RedirectToAction("Index", "Home");

        return View(order);
    }

    public async Task<IActionResult> MyOrders()
    {
        var userId = _userManager.GetUserId(User);

        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.ShippingAddress)
            .Include(o => o.PaymentMethod)
            .Include(o => o.Status)
            .ToListAsync();

        return View(orders);
    }

    public async Task<IActionResult> CancelOrder(int? orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Status)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        
        if (order == null)
            return RedirectToAction("Index", "Home");

        // Add status validation
        if (order.StatusId != 1 && order.StatusId != 2) // Only allow cancel for Pending/Confirmed
            return RedirectToAction("MyOrders");
        
        order.StatusId = 5;
        
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return RedirectToAction("MyOrders");
    }
}