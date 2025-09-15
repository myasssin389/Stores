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
                    .ThenInclude(p => p.Store)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        
        if (cart == null || !cart.CartProductMaps.Any())
            return RedirectToAction("Index", "Home");
        
        if (!ModelState.IsValid)
        {
            model.PaymentMethods = await _context.PaymentMethods.ToListAsync();
            model.Cart = cart;
            return View("~/Views/Cart/Checkout.cshtml", model);
        }
        
        var shippingAddress = await GetOrCreateAddressAsync(model.ShippingAddress);
        Address billingAddress = null;

        if (model.SelectedPaymentMethodId == 3) // Cash on Delivery
        {
            billingAddress = model.BillingAddress == null 
                ? shippingAddress 
                : await GetOrCreateAddressAsync(model.BillingAddress);
        }
        
        var productsGroupedByStoreId = cart.CartProductMaps.GroupBy(cpm => cpm.Product.StoreId);
        var createdOrders = new List<Order>();
        foreach (var storeGroup in productsGroupedByStoreId)
        {
            var orderItems = storeGroup.Select(sg => new OrderItem
            {
                ProductId = sg.ProductId,
                Quantity = sg.Quantity,
                Product = sg.Product
            }).ToList();
            
            var order = new Order
            {
                UserId = userId,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                PaymentMethodId = model.SelectedPaymentMethodId,
                Date = DateTime.UtcNow,
                OrderItems = orderItems,
                Status = OrderStatus.Pending,
                Total = GetTotalAmount(orderItems),
                StoreId = storeGroup.Key
            };
            
            _context.Orders.Add(order);
            createdOrders.Add(order);
        }
        
        _context.CartProductMaps.RemoveRange(cart.CartProductMaps);
        
        await _context.SaveChangesAsync();
        
        var orderIds = createdOrders.Select(o => o.Id).ToList();
        
        return RedirectToAction("OrderConfirmation", new { orderIds = string.Join(",", orderIds) });
    }

    public decimal GetTotalAmount(List<OrderItem> orderItems)
    {
        decimal totalAmount = 0;
        foreach (var item in orderItems)
        {
            totalAmount += item.Quantity * item.Product.Price;
        }

        return totalAmount;
    }

    public async Task<Address> GetOrCreateAddressAsync(Address input)
    {
        if (input == null)
            return null;
        
        var existingAddress = await _context.Addresses.FirstOrDefaultAsync(a =>
            a.City.Trim().ToLower() == input.City.Trim().ToLower() &&
            a.StreetName.Trim().ToLower() == input.StreetName.Trim().ToLower() &&
            a.StreetNumber.Trim() == input.StreetNumber.Trim() &&
            a.BuildingNumber.Trim() == input.BuildingNumber.Trim() &&
            a.ApartmentNumber.Trim() == input.ApartmentNumber.Trim());


        if (existingAddress != null)
            return existingAddress;
        
        _context.Addresses.Add(input);
        return input;
    }
    
    public async Task<IActionResult> OrderConfirmation(string orderIds)
    {
        var ids = orderIds.Split(',').Select(int.Parse).ToList();
        var orders = _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Store)
            .Include(o => o.ShippingAddress)
            .Include(o => o.BillingAddress)
            .Include(o => o.PaymentMethod)
            .Where(o => ids.Contains(o.Id))
            .ToList();

        return View(orders);
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
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        
        if (order == null)
            return RedirectToAction("Index", "Home");

        if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Confirmed)
            return RedirectToAction("MyOrders");
        
        order.Status = OrderStatus.Cancelled;
        foreach (var item in order.OrderItems)
            item.Product.Quantity += item.Quantity;
        
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        return RedirectToAction("MyOrders");
    }

    public IActionResult ManageOrders(int storeId)
    {
        if (storeId == 0)
            return RedirectToAction("ViewStore", "Store");

        var orders = _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Store)
            .Include(o => o.ShippingAddress)
            .Include(o => o.BillingAddress)
            .Include(o => o.PaymentMethod)
            .Include(o => o.Status)
            .Where(o => o.StoreId == storeId)
            .ToList();

        return View(orders);
    }

    public IActionResult OrderDetails(int? orderId)
    {
        if (orderId == null)
            return NotFound("Order not found");
        
        var order = _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Store)
            .Include(o => o.ShippingAddress)
            .Include(o => o.BillingAddress)
            .Include(o => o.PaymentMethod)
            .Include(o => o.Status)
            .FirstOrDefault(o => o.Id == orderId);
        
        return View(order);
    }
}