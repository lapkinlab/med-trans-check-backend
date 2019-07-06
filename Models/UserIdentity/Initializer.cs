using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models.Roles;
using Models.Users;

namespace Models.UserIdentity
{
    public static class Initializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            const string adminRole = "admin";
            const string userRole = "user";
            const string medicRole = "medic";
            const string mechanicRole = "mechanic";
            const string dispatcherRole = "dispatcher";

            if (await roleManager.FindByNameAsync(adminRole) == null)
            {
                await roleManager.CreateAsync(new Role(adminRole));
            }
            if (await roleManager.FindByNameAsync(userRole) == null)
            {
                await roleManager.CreateAsync(new Role(userRole));
            }
            if (await roleManager.FindByNameAsync(medicRole) == null)
            {
                await roleManager.CreateAsync(new Role(medicRole));
            }
            if (await roleManager.FindByNameAsync(mechanicRole) == null)
            {
                await roleManager.CreateAsync(new Role(mechanicRole));
            }
            if (await roleManager.FindByNameAsync(dispatcherRole) == null)
            {
                await roleManager.CreateAsync(new Role(dispatcherRole));
            }

            await RegisterUser(userManager, adminRole);
            await RegisterUser(userManager, medicRole);
            await RegisterUser(userManager, mechanicRole);
            await RegisterUser(userManager, dispatcherRole);
        }

        private static async Task RegisterUser(UserManager<User> userManager, string userRole)
        {
            const string email = "@lapkisoft.me";
            const string password = "qwe123";
            const string phoneNumber = "8-800-000-00-00";
            
            if (await userManager.FindByNameAsync(userRole) == null)
            {
                var dateTime = DateTime.UtcNow;
                
                var user = new User
                {
                    UserName = userRole,
                    Name = string.Empty,
                    Email = $"{userRole}{email}",
                    PhoneNumber = phoneNumber,
                    RegisteredAt = dateTime,
                    LastUpdateAt = dateTime
                };

                var result = await userManager.CreateAsync(user, password);
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, userRole);
                }
            }
        }
    }
}