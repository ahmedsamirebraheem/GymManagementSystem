using GymManagementDataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementDataAccessLayer.Data.DataSeeding;

public class IdentityDbContextSeeding
{
    public async static Task<bool> SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        try
        {
            var HasUser = await userManager.Users.AnyAsync();
            var HasRole = await roleManager.Roles.AnyAsync();
            if (HasRole && HasUser)
            {
                return false;
            }
            if (!HasRole)
            {
                var Roles = new List<IdentityRole>
                {
                    new(){Name = "SuperAdmin"},
                    new(){Name = "Admin"}
                };
                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        roleManager.CreateAsync(role).Wait();
                    }
                }
            }
            if (!HasUser)
            {
                var MainAdmin = new ApplicationUser()
                {
                    FirstName = "Ahmed",
                    LastName = "Samir",
                    UserName = "AhmedSamir",
                    Email = "AhmedSuperAdmin@gmail.com",
                    PhoneNumber = "01000349539"
                };

                await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");

                var Admin = new ApplicationUser()
                {
                    FirstName = "Karim",
                    LastName = "Samir",
                    UserName = "KarimSamir",
                    Email = "KarimAdmin@gmail.com",
                    PhoneNumber = "01288849606"
                };

                await userManager.CreateAsync(Admin, "P@ssw0rd");
                await userManager.AddToRoleAsync(Admin, "Admin");
            }
            return true;
        }
        catch (Exception e)
        {
            {
                Console.WriteLine($"Seed Failed! {e}");
                return false;
            }
        }
    }
}
