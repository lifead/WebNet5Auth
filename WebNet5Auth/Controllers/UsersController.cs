using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebNet5Auth.ViewModels.Identity;
using System.Linq;
using WebNet5Auth.Models.Identity;

namespace WebNet5Auth.Controllers
{
    /// <summary>
    /// Тестовый контроллер для проверки работы доступа по ролям
    /// </summary>
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public UsersController(UserManager<IdentityUser> UserManager)
        {
            userManager = UserManager;
        }

        public IActionResult Index()
        {
            var model = userManager.Users
                    .Select(x => new LoginViewModel { UserName = x.UserName })
                    .AsEnumerable();
            return View(model);
        }

        [Authorize(Roles = DefaultRoles.DefaultAdministrators)]
        public IActionResult Details()
        {
            var model = userManager.Users
                    .Select(x => new LoginViewModel { UserName = x.UserName, Email = x.Email })
                    .AsEnumerable();
            return View(model);
        }
    }
}
