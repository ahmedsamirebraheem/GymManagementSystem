using GymManagementBusinessLayer.Services.Interfaces;
using GymManagementBusinessLayer.ViewModels.AccountVM;
using GymManagementDataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementBusinessLayer.Services.Classes;

public class AccountServices(UserManager<ApplicationUser> userManager) : IAccountService
{
    public async Task<ApplicationUser?> ValidateUserAsync(LoginVM loginVM)
    {
        var user = await userManager.FindByEmailAsync(loginVM.Email);
        if(user is null)
        {
            return null;
        }
        var IsPasswordValid = await userManager.CheckPasswordAsync(user,loginVM.Password);
        return IsPasswordValid?user:null;
    }
}
