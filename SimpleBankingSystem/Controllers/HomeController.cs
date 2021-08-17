using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBankingSystem.Data;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;
using System;
using System.Linq;

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
        
            if (this.User.IsInRole("admin"))
            {
                return this.Redirect("/admin/adminhome/transactionsreview");
            }

            var model = this.IndexDashboardViewModelFiller();

            var errorReceivedData = (bool?)this.TempData["IsError"] ?? false;

            var messagesReceivedData = ((string[])this.TempData["Messages"]) ?? new string[0];

            model.SuccessOrError.IsError = errorReceivedData;

            model.SuccessOrError.AllMessages = messagesReceivedData;

            return this.View(model);
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public IActionResult Index(IndexDashboardPostModel model)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

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
                        using var transactionDb = this._context.Database.BeginTransaction();
                        try
                        {
                            user.BankAccount.Balance -= model.Ammount;

                            receiver.BankAccount.Balance += model.Ammount;

                            var transaction = new Transaction
                            {
                                Ammount = model.Ammount,
                                Date = DateTime.UtcNow,
                                Description = model.Description??"No description",
                                SenderBankAccId = user.BankAccount.Id,
                                ReceiverBankAccId = receiver.BankAccount.Id,
                            };

                            this._context.Transactions.Add(transaction);

                            this._context.SaveChanges();

                            transactionDb.Commit();
                        }
                        catch (Exception ex)
                        {
                            transactionDb.Rollback();
                            return this.Redirect("/home/error404");
                        }

                        this.TempData["IsError"] = false;

                        this.TempData["Messages"] = new string[] { $"Successfully sent {model.Ammount}$" };

                        return this.RedirectToAction("Index");
                    }

                    this.ModelState.AddModelError(string.Empty, "IBAN is incorrect");
                }

                else
                {
                    this.ModelState.AddModelError(string.Empty, "Balance too low");
                }
            }

            this.TempData["IsError"] = true;
            this.TempData["Messages"] = this._collector.ErrorCollector(this.ModelState).ToArray();


            return this.RedirectToAction("Index");
        }

        public IActionResult Error404()
        {
            return this.View();
        }

        private IndexDashboardViewModel IndexDashboardViewModelFiller ()
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var firstName = user.FirstName;

            var lastName = user.LastName;

            var photoUrl = user.PhotoUrl;

            var balance = user.BankAccount.Balance;

            var activeCards = user.BankAccount.Cards
                .Where(x => x.IsBlocked == false)
                .Where(x => DateTime.Compare(DateTime.UtcNow, x.ExpDate) <= 0)
                .ToList()
                .Count();

            var monthlyEarnings = user.BankAccount.ReceivedTransactions
                .Where(x => DateTime.Compare(x.Date, DateTime.UtcNow) <= 0
                 && DateTime.Compare(x.Date, new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)) >= 0)
                .Select(x => x.Ammount)
                .Sum();

            var iban = user.BankAccount.Iban;

            var model = new IndexDashboardViewModel
            {
                UserNavbarModel = new UserNavbarViewModel
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PhotoUrl = photoUrl,
                },
                Balanace = balance,
                ActiveCards = activeCards,
                MontlyEarnings = monthlyEarnings,
                Iban = iban,
                SuccessOrError = new SuccessOrErrorMessageForPartialViewModel(),
            };

            return model;
        }
    }
}
