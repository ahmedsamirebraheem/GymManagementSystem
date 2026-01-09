using GymManagementBusinessLayer.Services.Classes;
using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.TrainerVM;
using GymManagementDataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPresentationLayer.Controllers;

public class TrainerController(ITrainerService trainerService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var trainersVM = await trainerService.GetAllAsync();

        return View(trainersVM);
    }

    public async Task<IActionResult> Create()
    {

        var model = new CreateVM();
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> CreateTrainer(CreateVM createVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("MissingData", "Check data and Missing fields");
            return View("Create", createVM);
        }
       
        var isCreated = await trainerService.CreateAsync(createVM);
        if (!isCreated)
        {
            ModelState.AddModelError("CreationFailed", "Trainer creation failed. Please try again.");
            return View("Create", createVM);
        }
        TempData["SuccessMessage"] = "Trainer Created Successfully";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Trainer Id!";
            return RedirectToAction(nameof(Index));
        }

        var trainerDetails = await trainerService.GetDetailsAsync(id);

        if (trainerDetails == null)
        {
            TempData["ErrorMessage"] = $"No Member Found!";
            return RedirectToAction(nameof(Index));
        }
        return View(trainerDetails);
    }

    public async Task<IActionResult> Update(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Trainer Id!";
            return RedirectToAction(nameof(Index));
        }

        var trainer = await trainerService.GetUpdateAsync(id);

        if (trainer == null)
        {
            TempData["ErrorMessage"] = $"No Trainer Found!";
            return RedirectToAction(nameof(Index));
        }
        return View(trainer);
    }
    public async Task<IActionResult> UpdateTrainer(int id, UpdateVM updateVM)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Trainer Id!";
            return RedirectToAction(nameof(Index));
        }
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("MissingData", "Check data and Missing fields");
            return View("Update", updateVM);
        }

        var trainer = await trainerService.UpdateAsync(id,updateVM);

        if(trainer == false)
        {
            TempData["ErrorMessage"] = "Error while Updatind Trainer's data!";
            return View("Update", updateVM);
        }

        TempData["SuccessMessage"] = "Trainer Updated Successfully";

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Trainer Id!";
            return RedirectToAction(nameof(Index));
        }
        var trainer = await trainerService.GetDetailsAsync(id);
        if (trainer == null)
        {
            TempData["ErrorMessage"] = $"No Trainer With Found!";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.TrainerId = trainer.Id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTrainer(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid Trainer Id!";
        }
        var isDeleted = await trainerService.DeleteAsync(id);
        if (!isDeleted)
        {
            TempData["ErrorMessage"] = $"Error occurred while deleting trainer!";
        }
        TempData["SuccessMessage"] = "Trainer Deleted Successfully";
        return RedirectToAction(nameof(Index));
    }
}
