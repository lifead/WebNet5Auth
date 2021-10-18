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
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
    }
}
