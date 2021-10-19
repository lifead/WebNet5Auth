using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNet5Auth.ViewModels.Identity;

namespace WebNet5Auth.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> SignInManager)
        {
            userManager = UserManager;
            signInManager = SignInManager;
        }

        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }

            var user = new IdentityUser() { UserName = Model.UserName };

            var register_result = await userManager.CreateAsync(user, Model.Password);
            if (register_result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in register_result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(Model);
        }

        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var login_result = await signInManager.PasswordSignInAsync(
                Model.UserName,
                Model.Password,
                false,
                false);

            if (login_result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Неверное имя пользователя, или пароль");
            return View(Model);
        }


        public async Task<IActionResult> Logout()
        {
            var user_name = User.Identity.Name;
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
