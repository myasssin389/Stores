using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stores.Data;
using Stores.Models;
using Stores.ViewModels;

namespace Stores.Controllers;

public class StoreController : Controller
{
    private StoresDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public StoreController(StoresDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
    public async Task<IActionResult> Create(int applicationId)
    {
        var application = await _context.StoreAccountApplications.FindAsync(applicationId);
        if (application == null || application.VerificationStatusId != 1)
            return BadRequest("Application not found or already processed.");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
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

            var user = await _context.Users.FindAsync(application.StoreAdminId);
            await SetNewUserRoleAsync(user);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return RedirectToAction("Index", "Home");
        }
        catch
        {
            await transaction.RollbackAsync();
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    private async Task SetNewUserRoleAsync(ApplicationUser user)
    {
        if (user == null) return;
        
        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Any())
        {
            var removeResult = await _userManager
                .RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded) return;
        }

        var addResult = await _userManager.AddToRoleAsync(user, "StoreAdmin");
        if (!addResult.Succeeded) return;
    }
}