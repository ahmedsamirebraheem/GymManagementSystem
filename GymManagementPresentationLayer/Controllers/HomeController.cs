using GymManagementBusinessLayer.ViewModels.AnalyticsVM;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPresentationLayer.Controllers;

public class HomeController(IAnalyticsService analyticsService) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await analyticsService.GetAnalyticsDataAsync());
    }
}
