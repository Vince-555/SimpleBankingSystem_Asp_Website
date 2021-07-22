using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleBankingSystem.Data;
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
        private readonly IErrorCollector _collector;
        private readonly SBSDbContext _context;

        public HomeController(UserManager<ApplicationUser> userManager,IGetUserService getUserService,
            IErrorCollector collector, SBSDbContext context)
        {
            this._userManager = userManager;
            this._getUserService = getUserService;
            this._collector = collector;
            this._context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var model = this.IndexDashboardViewModelFiller();

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(IndexDashboardPostModel model)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var modelToPass = this.IndexDashboardViewModelFiller();

            if (this.ModelState.IsValid)
            {
                if (model.Ammount <= user.BankAccount.Balance)
                {
                    var receiver = this._userManager.Users
                    .Include(x => x.BankAccount)
                    .Where(x => x.BankAccount.Iban == model.ReceiverIban)
                    .FirstOrDefault();

                    if (receiver != null 
                        && user.BankAccount.Iban!=receiver.BankAccount.Iban)
                    {
                        user.BankAccount.Balance -= model.Ammount;

                        receiver.BankAccount.Balance += model.Ammount;

                        var transaction = new Transaction
                        {
                            Ammount = model.Ammount,
                            Date = DateTime.UtcNow,
                            Description = model.Description,
                            SenderBankAccId = user.BankAccount.Id,
                            ReceiverBankAccId = receiver.BankAccount.Id,
                        };

                        this._context.Transactions.Add(transaction);

                        this._context.SaveChanges();

                        modelToPass.Balanace -= model.Ammount;

                        modelToPass.SuccessOrError = new SuccessOrErrorMessageForPartialViewModel
                        {
                            IsError = false,
                            AllMessages = new List<string> { $"Successfully sent {model.Ammount}$" }
                        };

                        return this.View(modelToPass);
                    }

                    this.ModelState.AddModelError(string.Empty, "IBAN is incorrect");
                }

                else
                {
                    this.ModelState.AddModelError(string.Empty, "Balance too low");
                }
            }

            modelToPass.SuccessOrError = new SuccessOrErrorMessageForPartialViewModel
            {
                IsError = true,
                AllMessages = this._collector.ErrorCollector(this.ModelState)
            };

            return this.View(modelToPass);
        }

        public IActionResult TestError()
        {
            SuccessOrErrorMessageForPartialViewModel model = null;
            return this.View(model);
           
        }

        private IndexDashboardViewModel IndexDashboardViewModelFiller ()
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var firstName = user.FirstName;

            var lastName = user.LastName;

            var photoUrl = user.PhotoUrl;

            var balance = user.BankAccount.Balance;

            var activeCards = user.BankAccount.Cards.Where(x => x.IsBlocked == false).ToList().Count();

            var monthlyEarnings = user.BankAccount.ReceivedTransactions
                .Where(x => DateTime.Compare(x.Date, DateTime.UtcNow) <= 0
                 && DateTime.Compare(x.Date, new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)) >= 0)
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

            return model;
        }
    }
}
