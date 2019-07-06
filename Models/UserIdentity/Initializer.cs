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

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new Role("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new Role("user"));
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