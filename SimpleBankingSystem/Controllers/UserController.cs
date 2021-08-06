using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystem.Data;
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
        private readonly SBSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IErrorCollector _collector;

        public UserController(UserManager<ApplicationUser> userManager,
                                      SignInManager<ApplicationUser> signInManager,
                                      SBSDbContext context,
                                      IErrorCollector collector)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._collector = collector;
            this._context = context;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole("admin"))
                {
                    return this.Redirect("/admin/adminhome/transactionsreview");
                }

                return this.Redirect("/home/index");
            }

            var errorReceivedData = (bool?)this.TempData["IsError"] ?? false;

            var messagesReceivedData = ((string[])this.TempData["Messages"]) ?? new string[0];

            var SuccessOrError = new SuccessOrErrorMessageForPartialViewModel
            {
                IsError = errorReceivedData,
                AllMessages = messagesReceivedData,
            };

            return this.View(SuccessOrError);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole("admin"))
                {
                   return this.RedirectToAction("/admin/adminhome/transactionsreview");
                }

                return this.Redirect("/home/index");
            }

            if (this.ModelState.IsValid)
            {
                bool rememberMe = model.RememberMe == "on" ? true : false;

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, rememberMe, false);

                if (result.Succeeded)
                {
                    if (this.User.IsInRole("admin"))
                    {
                        return this.Redirect("/admin/adminhome/transactionsreview");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            this.TempData["IsError"] = true;

            this.TempData["Messages"] = _collector.ErrorCollector(this.ModelState).ToArray();

            return this.RedirectToAction("Login");    
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole("admin"))
                {
                    return this.Redirect("/admin/adminhome/transactionsreview");
                }

                return this.Redirect("/home/index");
            }

            var errorReceivedData = (bool?)this.TempData["IsError"] ?? false;

            var messagesReceivedData = ((string[])this.TempData["Messages"]) ?? new string[0];

            var SuccessOrError = new SuccessOrErrorMessageForPartialViewModel
            {
                IsError = errorReceivedData,
                AllMessages = messagesReceivedData,
            };

            return this.View(SuccessOrError);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole("admin"))
                {
                    return this.Redirect("/admin/adminhome/transactionsreview");
                }

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
                    await this._userManager.AddToRoleAsync(user, "user");

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Login", "User");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            this.TempData["IsError"] = true;

            this.TempData["Messages"] = _collector.ErrorCollector(this.ModelState).ToArray();

            return this.RedirectToAction("Register");
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole("admin"))
                {
                    return this.Redirect("/admin/adminhome/transactionsreview");
                }

                return this.Redirect("/home/index");
            }

            var errorReceivedData = (bool?)this.TempData["IsError"] ?? false;

            var messagesReceivedData = ((string[])this.TempData["Messages"]) ?? new string[0];

            var SuccessOrError = new SuccessOrErrorMessageForPartialViewModel
            {
                IsError = errorReceivedData,
                AllMessages = messagesReceivedData,
            };


            return this.View(SuccessOrError);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ForgotPassword(ForgotPasswordFormModel model) //NOT FUNCTIONAL !!!
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole("admin"))
                {
                    return this.Redirect("/admin/adminhome/transactionsreview");
                }

                return this.Redirect("/home/index");
            }

            this.TempData["Messages"] = new string[] { $"An email has been sent to {model.Email}" };

            return this.RedirectToAction("ForgotPassword");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login","User");
        }
    }
}
