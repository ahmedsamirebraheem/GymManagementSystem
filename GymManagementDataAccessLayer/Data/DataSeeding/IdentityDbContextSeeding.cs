using GymManagementDataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymManagementDataAccessLayer.Data.DataSeeding
{
    public class IdentityDbContextSeeding
    {
        public async static Task<bool> SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                // 1. Seed Roles
                var roles = new[] { "SuperAdmin", "Admin" };
                foreach (var roleName in roles)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // 2. Seed SuperAdmin (Ahmed)
                await CreateOrUpdateUser(userManager, "AhmedSuperAdmin@gmail.com", "Ahmed", "Samir", "AhmedSamir", "01000349539", "SuperAdmin");

                // 3. Seed Admin (Karim)
                await CreateOrUpdateUser(userManager, "KarimAdmin@gmail.com", "Karim", "Samir", "KarimSamir", "01288849606", "Admin");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Seed Failed! {e.Message}");
                return false;
            }
        }

        // الميثود دي لازم تكون جوه الكلاس وبرا الميثود اللي فوق
        private async static Task CreateOrUpdateUser(UserManager<ApplicationUser> userManager, string email, string fName, string lName, string uName, string phone, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    FirstName = fName,
                    LastName = lName,
                    UserName = uName,
                    Email = email,
                    PhoneNumber = phone,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, "P@ssw0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
            else
            {
                // تحديث بسيط بدون تغيير الباسورد عشان الـ Login ميفشلش
                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    await userManager.UpdateAsync(user);
                }

                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}