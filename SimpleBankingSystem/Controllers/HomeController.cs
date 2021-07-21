using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetUserService _getUserService;

        public HomeController(UserManager<ApplicationUser> userManager,IGetUserService getUserService)
        {
            this._userManager = userManager;
            this._getUserService = getUserService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var firstName = user.FirstName;

            var lastName = user.LastName;

            var photoUrl = user.PhotoUrl;

            var balance = user.BankAccount.Balance;

            var activeCards = user.BankAccount.Cards.Where(x => x.IsBlocked == false).ToList().Count();

            var monthlyEarnings = user.BankAccount.Transactions
                .Where(x => x.ReceiverId == user.Id)
                .Where(x => DateTime.Compare(x.Date, DateTime.Now) <= 0
                 && DateTime.Compare(x.Date, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)) >= 0)
                .Select(x => x.Ammount)
                .Sum();

            var iban = user.BankAccount.Iban;

            var model = new IndexDashboardViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                PhotoUrl = photoUrl,
                Balanace = balance,
                ActiveCards = activeCards,
                MontlyEarnings = monthlyEarnings,
                Iban = iban,
                SuccessOrError = null,
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(IndexDashboardPostModel model)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            if (this.ModelState.IsValid)
            {

            }
            return this.View();
        }

        public IActionResult TestError()
        {
            SuccessOrErrorMessageForPartialViewModel model = null;
            return this.View(model);
           
        }
    }
}
