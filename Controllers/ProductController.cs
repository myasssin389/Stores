using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stores.Data;
using Stores.Models;
using Stores.ViewModels;

namespace Stores.Controllers;

public class ProductController : Controller
{
    private StoresDbContext _context;
    private UserManager<ApplicationUser> _userManager;

    public ProductController(StoresDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [Authorize(Roles = "StoreAdmin")]
    public IActionResult Create()
    {
        var viewModel = new ProductFormViewModel();
        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        
        var userId = _userManager.GetUserId(User);
        
        var store = _context.Stores.SingleOrDefault(s => s.StoreAdminId == userId);
        if (store == null)
            return Unauthorized();

        var product = new Product()
        {
            Name = viewModel.Name,
            Description = viewModel.Description,
            Price = viewModel.Price,
            Quantity = viewModel.Quantity,
            PhotoLink = viewModel.PhotoLink,
            StoreId = store.Id
        };
        
        _context.Products.Add(product);
        _context.SaveChanges();
        
        return RedirectToAction("Index", "Home");
    }
}