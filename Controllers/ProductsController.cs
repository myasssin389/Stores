using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stores.Data;
using Stores.Models;
using Stores.ViewModels;

namespace Stores.Controllers;

public class ProductsController : Controller
{
    private StoresDbContext _context;

    public ProductsController(StoresDbContext context)
    {
        _context = context;
    }
    
    public IActionResult Create()
    {
        var viewModel = new ProductFormViewModel()
        {
            Stores = _context.Stores.ToList()
        };
        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductFormViewModel viewModel)
    {
        viewModel.Stores = _context.Stores.ToList();
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var product = new Product()
        {
            Name = viewModel.Name,
            Description = viewModel.Description,
            Price = viewModel.Price,
            PhotoLink = viewModel.PhotoLink,
            StoreId = viewModel.Store
        };
        
        _context.Products.Add(product);
        _context.SaveChanges();
        
        return RedirectToAction("Index", "Home");
    }
}