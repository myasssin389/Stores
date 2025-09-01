using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stores.Data;
using Stores.Models;
using Stores.ViewModels;

namespace Stores.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private StoresDbContext _context;

    public HomeController(ILogger<HomeController> logger)
    {
        _context = new StoresDbContext();
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Include(c => c.Stores)
            .ThenInclude(s => s.Products)
            .ToListAsync();

        var viewModel = categories.Select(c => new CategoryViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Stores = c.Stores.Select(s => new StoreViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Products = s.Products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Description = p.Description,
                    PhotoLink = p.PhotoLink
                }).ToList()
            }).ToList()
        }).ToList();

        return View(viewModel);
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}