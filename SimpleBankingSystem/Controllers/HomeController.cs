using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleBankingSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                this.RedirectToPage("login.cshtml");
            }
            
            return this.View();
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginFormModel model)
        {
            return this.View();
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterFormModel model)
        {
            return this.View();
        }

        public IActionResult ForgotPassword()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordFormModel model)
        {
            return this.View();
        }

        public IActionResult TestError()
        {
            SuccessOrErrorMessageForPartialViewModel model = null;
            return this.View(model);
           
        }
    }
}
