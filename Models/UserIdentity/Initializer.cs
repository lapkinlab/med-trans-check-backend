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
            const string userName = "admin";
            const string email = "admin@lapkisoft.me";
            const string password = "qwe123";
            const string phoneNumber = "8-800-000-00-00";
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

            var dateTime = DateTime.UtcNow;

            if (await userManager.FindByNameAsync(userName) == null)
            {
                var admin = new User
                {
                    UserName = userName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    RegisteredAt = dateTime,
                    LastUpdatedAt = dateTime
                };

                var result = await userManager.CreateAsync(admin, password);
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}