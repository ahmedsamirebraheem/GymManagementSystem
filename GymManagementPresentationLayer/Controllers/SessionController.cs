using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.SessionVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPresentationLayer.Controllers;

public class SessionController(ISessionService sessionService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var sessions = await sessionService.GetAllAsync();
        return View(sessions);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await sessionService.GetCategoriesForDropdown();
        ViewBag.Categories = new SelectList( categories,"Id","Name" );
        var trainers = await sessionService.GetTrainersForDropdown();
        ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateVM createVM)
    {
        if(!ModelState.IsValid)
        {
          await  LoadTrainersDropDown();
          await LoadCategoriesDropDown();


            return View(createVM);
        }
        var sessionIsCreated =await sessionService.CreateAsync(createVM);
        if(!sessionIsCreated)
        {
            await LoadTrainersDropDown();
            await LoadCategoriesDropDown();

            return View(createVM);
        
        }
        TempData["SuccessMessage"] = "Session created successfully!";

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Details(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Session Id!";
            return RedirectToAction(nameof(Index));
        }
        var sessionDetails = await sessionService.GetAsync(id);
        if (sessionDetails == null)
        {
            TempData["ErrorMessage"] = $"No Session Found!";
            return RedirectToAction(nameof(Index));
        }
        return View(sessionDetails);
    }

    public async Task<IActionResult> Update(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Session Id!";
            return RedirectToAction(nameof(Index));
        }
        var sessionDetails = await sessionService.GetUpdateAsync(id);
        if (sessionDetails == null)
        {
            TempData["ErrorMessage"] = $"Session Not Found!";
            return RedirectToAction(nameof(Index));
        }
        await LoadTrainersDropDown();
        await LoadCategoriesDropDown();
        return View(sessionDetails);

    }
    [HttpPost]
    public async Task<IActionResult> UpdateSession(int id, UpdateVM updateVM)
    {
        if (id != updateVM.Id)
        {
            TempData["ErrorMessage"] = "Data mismatch error.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await LoadTrainersDropDown();
            await LoadCategoriesDropDown();
            return View("Update", updateVM);
        }

        var isUpdated = await sessionService.UpdateAsync(id, updateVM);

        if (!isUpdated)
        {
            TempData["ErrorMessage"] = "Could not update session. Check trainer availability or capacity constraints.";
            await LoadTrainersDropDown();
            await LoadCategoriesDropDown();
            return View("Update", updateVM);
        }

        TempData["SuccessMessage"] = "Session updated successfully!";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Session Id!";
            return RedirectToAction(nameof(Index));
        }
        var sessionDetails = await sessionService.GetAsync(id);
        if (sessionDetails == null)
        {
            TempData["ErrorMessage"] = $"Session Not Found!";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.SessionId = id;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> DeleteSession(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Session Id!";
            return RedirectToAction(nameof(Index));
        }
        var isDeleted = await sessionService.DeleteAsync(id);
        if (!isDeleted)
        {
            TempData["ErrorMessage"] = "Could not delete session. It have associated bookings.";
            return RedirectToAction(nameof(Delete), new { id });
        }
        TempData["SuccessMessage"] = "Session deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
    private async Task LoadCategoriesDropDown()
    {
        var categories = await sessionService.GetCategoriesForDropdown();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
    }
    private async Task LoadTrainersDropDown()
    {
        var trainers = await sessionService.GetTrainersForDropdown();
        ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
    }
    public async Task<JsonResult> GetTrainersByCategory(int categoryId)
    {
        // جلب كل المدربين (المابنج الجديد سيملأ الـ SpecializationId تلقائياً)
        var trainers = await sessionService.GetTrainersForDropdown();

        // فلترة المدربين بناءً على القسم المختار
        var filtered = trainers
            .Where(t => t.SpecializationId == categoryId)
            .Select(t => new {
                id = t.Id,
                name = t.Name
            })
            .ToList();

        return Json(filtered);
    }

}
