using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Controllers
{
    public class UserController :Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IErrorCollector _collector;

        public UserController(UserManager<ApplicationUser> userManager,
                                      SignInManager<ApplicationUser> signInManager,
                                      IErrorCollector collector)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._collector = collector;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/home/index");
            }

            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/home/index");
            }

            if (this.ModelState.IsValid)
            {
                bool rememberMe = model.RememberMe == "on" ? true : false;

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, rememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            
            return this.View(new SuccessOrErrorMessageForPartialViewModel
            {
                IsError = true,
                AllMessages = _collector.ErrorCollector(this.ModelState)
            });
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/home/index");
            }

            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/home/index");
            }

            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                user.BankAccount = new BankAccount
                {
                    UserId = user.Id
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Login", "User");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return this.View(new SuccessOrErrorMessageForPartialViewModel 
            { 
                IsError = true,
                AllMessages = _collector.ErrorCollector(this.ModelState)
            });
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/home/index");
            }

            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ForgotPassword(ForgotPasswordFormModel model) //NOT FUNCTIONAL !!!
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/home/index");
            }

            var message = $"An email has been sent to {model.Email}";

            return this.View(new SuccessOrErrorMessageForPartialViewModel 
            { 
                IsError = false,
                AllMessages = new List<string> { message }
            });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
