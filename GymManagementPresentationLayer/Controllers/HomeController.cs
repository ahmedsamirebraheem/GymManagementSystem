using GymManagementBusinessLayer.ViewModels.AnalyticsVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPresentationLayer.Controllers;

[Authorize(Roles= "SuperAdmin,Admin")]
public class HomeController(IAnalyticsService analyticsService) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await analyticsService.GetAnalyticsDataAsync());
    }
}
