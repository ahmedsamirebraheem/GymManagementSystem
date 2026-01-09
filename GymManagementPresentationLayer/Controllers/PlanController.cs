using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.PlanVM;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPresentationLayer.Controllers;

public class PlanController(IPlanService planService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var plans = await planService.GetAllAsync();

        return View(plans);
    }
    public async Task<IActionResult> Details(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Plan Id!";
            return RedirectToAction(nameof(Index));
        }
        var planDetails = await planService.GetAsync(id);
        if (planDetails == null)
        {
            TempData["ErrorMessage"] = $"No Plan Found!";
            return RedirectToAction(nameof(Index));
        }
        return View(planDetails);
        
    }

    public async Task<IActionResult> Update(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Plan Id!";
            return RedirectToAction(nameof(Index));
        }
        var planUpdate = await planService.GetUpdateAsync(id);
        if (planUpdate == null)
        {
            TempData["ErrorMessage"] = $"No Plan Found!";
            return RedirectToAction(nameof(Index));
        }
        return View(planUpdate);

    }

    [HttpPost]
    public async Task<IActionResult> UpdatePaln(int id, UpdateVM updateVM)
    {
        if (id <= 0) 
        {
            TempData["ErrorMessage"] = "No Plan Found.";

            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            TempData["WrongData"] = "Invalid Plan Id!";

            return View("Update", updateVM);
        }
        var isUpdated = await planService.UpdateAsync(id, updateVM);
        if (!isUpdated)
        {
            TempData["ErrorMessage"] = "Plan could not be updated! It have active memberships.";
            return RedirectToAction(nameof(Index));
        }
        TempData["SuccessMessage"] = "Plan updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Plan Id!";
            return RedirectToAction(nameof(Index));
        }
        var isToggled = await planService.TogglStatus(id);
        if (!isToggled)
        {
            TempData["ErrorMessage"] = "Plan status could not be changed!";
            return RedirectToAction(nameof(Index));
        }
        TempData["SuccessMessage"] = "Plan status changed successfully!";
        return RedirectToAction(nameof(Index));
    }

} 
