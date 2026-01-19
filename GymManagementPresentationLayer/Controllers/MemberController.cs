using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.Services.Interfaces.AttachmentService;
using GymManagementBusinessLayer.ViewModels.MemberVM;
using GymManagementDataAccessLayer.Entities;
using GymManagementDataAccessLayer.Repositories.Classes;
using GymManagementDataAccessLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace GymManagementPresentationLayer.Controllers;

public class MemberController(IMemberService memberService, IAttachmentService attachmentService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var members = await memberService.GetAllAsync();
        return View(members);
    }
    public async Task<IActionResult> Details(int id)
    {
        if(id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Member Id!"; 
            return RedirectToAction(nameof(Index));
        }

        var membersDetails = await memberService.GetDetailsAsync(id);

        if(membersDetails == null)
        {
            TempData["ErrorMessage"] = $"No Member Found!";
            return RedirectToAction(nameof(Index));
        }
        return View(membersDetails);
    }
    public async Task<IActionResult> HealthRecordDetails(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Member Id!";

            return RedirectToAction(nameof(Index));
        }

        var healthDetails = await memberService.GetHealthRecordAsync(id);

        if (healthDetails == null)
        {
            TempData["ErrorMessage"] = $"No Member Found!";

            return RedirectToAction(nameof(Index));
        }
        return View(healthDetails);
    }

    public async Task<IActionResult> Create()
    {
        var model = new CreateVM
        {
            HealthRecord = new HealthRecordVM(),
            PhotoFile = null!
        };

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> CreateMember(CreateVM create)
    {
        if(!ModelState.IsValid)
        {
            ModelState.AddModelError("MissingData","Check data and Missing fields");
            return View(nameof(Create),create);
        }
        var isCreated = await memberService.CreateAsync(create);
        if(!isCreated)
        {
            TempData["ErrorMessage"] = "Error occurred while creating member!";
            return View(nameof(Create),create);
        }
        TempData["SuccessMessage"] = "Member Created Successfully";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Member Id!";
            return RedirectToAction(nameof(Index));
        }
        var updateVM = await memberService.GetUpdateAsync(id);

        if (updateVM == null)
        {
            TempData["ErrorMessage"] = $"No Member Found!";
            return RedirectToAction(nameof(Index));
        }
        return View(updateVM);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id,UpdateVM updateVM)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Member Id!";
            return RedirectToAction(nameof(Index));
        }
        if (!ModelState.IsValid)
        {
            return View(nameof(Update), updateVM);
        }
       
        var isMemberUpdated = await memberService.UpdateAsync(id, updateVM);
        if (!isMemberUpdated)
        {
            TempData["ErrorMessage"] = "Error occurred while updating member!";
            return RedirectToAction(nameof(Update), new { id });
        }
        TempData["SuccessMessage"] = "Member Updated Successfully";
        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Member Id!";
            return RedirectToAction(nameof(Index));
        }
        var member = await memberService.GetDetailsAsync(id);
        if (member == null)
        {
            TempData["ErrorMessage"] = $"No Member Found!";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MemberId = member.Id;
        return View();

    }
    [HttpPost]
    public async Task<IActionResult> DeleteMember([FromForm]int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Member Id!";
            
        }
        var member = await memberService.DeleteAsync(id);
        if (!member)
        {
            TempData["ErrorMessage"] = $"Error occurred while deleting member!";

        }
        else
        {          
            TempData["SuccessMessage"] = "Member Deleted Successfully";
        }

        return RedirectToAction(nameof(Index));

    }
}
