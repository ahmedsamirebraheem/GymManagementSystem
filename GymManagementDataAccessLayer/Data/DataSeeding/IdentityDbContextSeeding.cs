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
            var HasRole = await roleManager.Roles.AnyAsync();
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

            // Check and create/update MainAdmin
            var mainAdmin = await userManager.FindByEmailAsync("AhmedSuperAdmin@gmail.com");
            if (mainAdmin == null)
            {
                mainAdmin = new ApplicationUser()
                {
                    FirstName = "Ahmed",
                    LastName = "Samir",
                    UserName = "AhmedSamir",
                    Email = "AhmedSuperAdmin@gmail.com",
                    PhoneNumber = "01000349539",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(mainAdmin, "P@ssw0rd");
                await userManager.AddToRoleAsync(mainAdmin, "SuperAdmin");
            }
            else
            {
                // Ensure email is confirmed and user has correct role
                if (!mainAdmin.EmailConfirmed)
                {
                    mainAdmin.EmailConfirmed = true;
                    await userManager.UpdateAsync(mainAdmin);
                }
                if (!await userManager.IsInRoleAsync(mainAdmin, "SuperAdmin"))
                {
                    await userManager.AddToRoleAsync(mainAdmin, "SuperAdmin");
                }
            }

            // Check and create/update Admin
            var admin = await userManager.FindByEmailAsync("KarimAdmin@gmail.com");
            if (admin == null)
            {
                admin = new ApplicationUser()
                {
                    FirstName = "Karim",
                    LastName = "Samir",
                    UserName = "KarimSamir",
                    Email = "KarimAdmin@gmail.com",
                    PhoneNumber = "01288849606",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "P@ssw0rd");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
            else
            {
                // Ensure email is confirmed and user has correct role
                if (!admin.EmailConfirmed)
                {
                    admin.EmailConfirmed = true;
                    await userManager.UpdateAsync(admin);
                }
                if (!await userManager.IsInRoleAsync(admin, "Admin"))
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
                // Reset password to ensure it's correct
                var token = await userManager.GeneratePasswordResetTokenAsync(admin);
                await userManager.ResetPasswordAsync(admin, token, "P@ssw0rd");
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
