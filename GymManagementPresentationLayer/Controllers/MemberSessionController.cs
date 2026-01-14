using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.MemberSessionVM;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPresentationLayer.Controllers;

public class MemberSessionController(IMemberSessionService memberSessionService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = await memberSessionService.GetAllAsync();
        return View(model);
    }

    // هذا هو الأكشن الموحد للحالتين
    public async Task<IActionResult> Details(int sessionId)
    {
        var model = await memberSessionService.GetAsync(sessionId);
        if (model == null) return NotFound();

        // تأكد أن اسم ملف الـ View هو Details.cshtml
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableMembers(int sessionId)
    {
        var model = await memberSessionService.PrepareCreateViewModelAsync(sessionId);
        var members = model.MembersList.Select(m => new { id = m.Value, name = m.Text });
        return Json(members);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create(CreateMemberSessionVM model)
    {
        var success = await memberSessionService.CreateAsync(model);
        return Json(new { success = success, message = success ? "" : "Member already registered!" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(int sessionId, int memberId)
    {
        await memberSessionService.CancelBookingAsync(sessionId, memberId);

        // إضافة رسالة للـ TempData
        TempData["SuccessMessage"] = "Booking cancelled successfully!";

        return RedirectToAction("Details", new { sessionId = sessionId });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> UpdateAttendance(int sessionId, int memberId)
    {
        var newStatus = await memberSessionService.ToggleAttendanceAsync(sessionId, memberId);
        return Json(new { success = true, isAttended = newStatus });
    }
}