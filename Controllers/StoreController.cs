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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(int applicationId)
    {
        var application = _context.StoreAccountApplications.Find(applicationId);
        if (application == null || application.VerificationStatusId != 1)
            return BadRequest("Application not found or already processed.");

        var store = new Store
        {
            Name = application.StoreName,
            Address = application.StoreAddress,
            City = application.StoreCity,
            Phone = application.StorePhone,
            Email = application.StoreEmail,
            StoreAdminId = application.StoreAdminId,
            CategoryId = application.StoreCategoryId,
            TaxRegistrationNumber = application.StoreTaxRegistrationNumber,
            CommercialRegistrationNumber = application.StoreCommercialRegistrationNumber
        };
        
        _context.Stores.Add(store);
        application.VerificationStatusId = 2;
        _context.SaveChanges();
        
        return RedirectToAction("Index", "Home");
    }
}