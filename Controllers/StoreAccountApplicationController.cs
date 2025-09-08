using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stores.Data;
using Stores.Models;
using Stores.ViewModels;

namespace Stores.Controllers;

public class StoreAccountApplicationController : Controller
{
    private StoresDbContext _context;
    private UserManager<ApplicationUser> _userManager;

    public StoreAccountApplicationController(StoresDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public IActionResult Apply()
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
    public IActionResult Apply(StoreFormViewModel viewModel)
    {
        viewModel.Categories = _context.Categories.ToList();
        if (!ModelState.IsValid)
            return View(viewModel);
        
        var userId = _userManager.GetUserId(User);

        var storeAccountApplication = new StoreAccountApplication
        {
            StoreName = viewModel.Name,
            StoreAddress = viewModel.Address,
            StoreCity = viewModel.City,
            StorePhone = viewModel.Phone,
            StoreEmail = viewModel.Email,
            StoreCategoryId = viewModel.Category,
            StoreAdminId = userId,
            VerificationStatusId = 1,
            StoreTaxRegistrationNumber = viewModel.TaxRegistrationNumber,
            StoreCommercialRegistrationNumber = viewModel.CommercialRegistrationNumber,
        };
        
        _context.StoreAccountApplications.Add(storeAccountApplication);
        _context.SaveChanges();
        
        return RedirectToAction("ApplicationConfirmation", new { applicationId = storeAccountApplication.Id });
    }

    public IActionResult ApplicationConfirmation(int? applicationId)
    {
        var userId = _userManager.GetUserId(User);

        var application =
            _context.StoreAccountApplications
                .FirstOrDefault(a => a.Id == applicationId && a.StoreAdminId == userId);
        
        if (application == null)
            return RedirectToAction("Index", "Home");
        
        return View(application);
    }
}