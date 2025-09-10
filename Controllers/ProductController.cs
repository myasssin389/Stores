using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [Authorize(Roles = "StoreAdmin")]
    public IActionResult Edit(int id)
    {
        var product = _context.Products
            .Include(p => p.Store)
            .SingleOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();
        
        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = "StoreAdmin")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product product)
    {
        
        if (!ModelState.IsValid)
        {
            Console.WriteLine("ModelState is invalid. Errors:");
            foreach (var modelError in ModelState)
            {
                foreach (var error in modelError.Value.Errors)
                {
                    Console.WriteLine($"Field: {modelError.Key}, Error: {error.ErrorMessage}");
                }
            }
            // Reload the product with Store data for the view
            var productWithStore = _context.Products
                .Include(p => p.Store)
                .SingleOrDefault(p => p.Id == product.Id);
        
            if (productWithStore == null)
                return NotFound();
        
            // Copy the form values to the reloaded product
            productWithStore.Name = product.Name;
            productWithStore.Description = product.Description;
            productWithStore.Price = product.Price;
            productWithStore.Quantity = product.Quantity;
            productWithStore.PhotoLink = product.PhotoLink;
        
            return View(productWithStore);
        }
        
        var productToUpdate = _context.Products
            .Include(p => p.Store)
            .SingleOrDefault(p => p.Id == product.Id);
        if (productToUpdate == null)
            return NotFound();
        
        productToUpdate.Name = product.Name;
        productToUpdate.Description = product.Description;
        productToUpdate.Price = product.Price;
        productToUpdate.Quantity = product.Quantity;
        productToUpdate.PhotoLink = product.PhotoLink;
        
        _context.SaveChanges();
        
        TempData["Success"] = "Product updated successfully";
        return RedirectToAction("ViewStore",
            "Store",
            new { userId = productToUpdate.Store.StoreAdminId });
    }
}