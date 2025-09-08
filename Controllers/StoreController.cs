using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stores.Data;
using Stores.Models;
using Stores.ViewModels;

namespace Stores.Controllers;

public class StoreController : Controller
{
    private StoresDbContext _context;

    public StoreController(StoresDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        var viewModel = new StoreFormViewModel()
        {
            Categories = _context.Categories.ToList(),
        };
        return View(viewModel);
    }
    
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(StoreFormViewModel viewModel)
    {
        viewModel.Categories = _context.Categories.ToList();
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var store = new Store
        {
            Name = viewModel.Name,
            Address = viewModel.Address,
            City = viewModel.City,
            Phone = viewModel.Phone,
            CategoryId = viewModel.Category
        };
        
        _context.Stores.Add(store);
        _context.SaveChanges();
        
        return RedirectToAction("Index", "Home");
    }
}