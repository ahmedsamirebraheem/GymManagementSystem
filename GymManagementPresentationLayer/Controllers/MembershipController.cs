using GymManagementBusinessLayer.Services.Classes;
using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.MembershipVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPresentationLayer.Controllers;

public class MembershipController(IMembershipService membershipService,IMemberService memberService,IPlanService planService ) : Controller
{
    public async Task<IActionResult> Index()
    {
        var memberships = await membershipService.GetAllAsync();
        return View(memberships);
    }
    public async Task<IActionResult> Create()
    {
        await LoadMembersDropdown();
        await LoadPlansDropdown();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateVM createVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("MissingData", "Check data and Missing fields");
            await LoadMembersDropdown();
            await LoadPlansDropdown();
            return View(createVM);
        }
        var result = await membershipService.CreateAsync(createVM);
        if (!result)
        {
            TempData["ErrorMessage"] = "Error occurred while creating member!";
            await LoadMembersDropdown();
            await LoadPlansDropdown();
            return View(nameof(Create), createVM);
        }
        TempData["SuccessMessage"] = "Member Created Successfully";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await membershipService.DeleteAsync(id);
        if (!result)
        {
            TempData["ErrorMessage"] = "Error occurred while Cancelling membership!";
            return RedirectToAction(nameof(Index));
        }
        TempData["SuccessMessage"] = "Membership Cancelled Successfully";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadMembersDropdown()
    {
        var members = await memberService.GetAllAsync();
        ViewBag.Members = new SelectList(members, "Id", "Name");

    }
    private async Task LoadPlansDropdown()
    {
        var plans = await planService.GetAllAsync();

        ViewBag.Plans = new SelectList(plans, "Id", "Name");
    }
}
