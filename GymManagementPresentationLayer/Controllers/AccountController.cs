using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.AccountVM;
using GymManagementDataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPresentationLayer.Controllers;

public class AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager) : Controller
{
   public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid) return View(loginVM);

        var user = await accountService.ValidateUserAsync(loginVM);
        if (user is null)
        {
            ModelState.AddModelError("InvalidLogin", "Invalid Username or Password");
            return View(loginVM);
        }

        var Result = await signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe,false);
        if (Result.IsNotAllowed)
        {
            ModelState.AddModelError("InvalidLogin", "Your Email is not Allowed");
        }
        if (Result.IsLockedOut)
        {
            ModelState.AddModelError("InvalidLogin", "Your Email is LockdOut");

        }
        if(Result.Succeeded)
            return RedirectToAction("Index","Home");
        return View(loginVM);
    }

    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
    public ActionResult AccessDenied()
    {
        return View();
    }
}
