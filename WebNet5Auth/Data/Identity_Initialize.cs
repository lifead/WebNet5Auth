using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNet5Auth.Models.Identity;

namespace WebNet5Auth.Data
{
    /// <summary>
    /// Инициализация БД ролями и пользваотелями "по умолчанию"
    /// </summary>
    public class Identity_Initialize
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public Identity_Initialize(RoleManager<IdentityRole> RoleManager, UserManager<IdentityUser> UserManager)
        {
            roleManager = RoleManager;
            userManager = UserManager;

            Initialize();
        }
        public void Initialize() => InitializeAsync().Wait();

        public async Task InitializeAsync()
        {
            await AddDefaultUserAndRole().ConfigureAwait(false);
        }

        /// <summary>
        /// Добавление ролей и пользователей "Default"
        /// </summary>
        /// <returns></returns>
        private async Task AddDefaultUserAndRole()
        {
            // Добавление ролей "по умолчанию"
            if (!await roleManager.RoleExistsAsync(DefaultRoles.DefaultAdministrators))
                await roleManager.CreateAsync(new IdentityRole { Name = DefaultRoles.DefaultAdministrators });

            if (!await roleManager.RoleExistsAsync(DefaultRoles.DefaultUsers))
                await roleManager.CreateAsync(new IdentityRole { Name = DefaultRoles.DefaultUsers });

            // Добавление пользователя с правами администратора
            if (await userManager.FindByNameAsync(DefaultUser.DefaultAdmin) is null)
            {
                var admin = new IdentityUser
                {
                    UserName = DefaultUser.DefaultAdmin,
                    Email = "admin@example.com"
                };

                var create_result = await userManager.CreateAsync(admin, DefaultUser.DefaultAdminPassword);
                if (create_result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, DefaultRoles.DefaultAdministrators);
                }
                else
                {
                    var errors = create_result.Errors.Select(error => error.Description);
                    throw new InvalidOperationException($"Ошибка при создании пользователя - Администратора: {string.Join(", ", errors)}");
                }
            }

            // Добавление пользователя с правами User
            if (await userManager.FindByNameAsync("test_default_user") is null)
            {
                var admin = new IdentityUser { UserName = "test_default_user", Email = "user@example.com" };
                var create_result = await userManager
                    .CreateAsync(admin, Guid.NewGuid().ToString().ToLower() + Guid.NewGuid().ToString().ToUpper());
                if (create_result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, DefaultRoles.DefaultUsers);
                }
            }

        }

    }
}
