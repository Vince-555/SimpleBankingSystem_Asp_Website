using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBankingSystem.Areas.Admin.Models;
using SimpleBankingSystem.Data;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;
using System;
using System.Linq;

namespace SimpleBankingSystem.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class AdminHomeController : Controller
    {
        private readonly SBSDbContext _context;
        private readonly IErrorCollector _collector;
        private readonly IGetTransactions _getTransactions;

        public AdminHomeController(SBSDbContext context,
            IErrorCollector collector,
            IGetTransactions getTransactions)
        {
            this._context = context;
            this._getTransactions = getTransactions;
            this._collector = collector;
        }

        public IActionResult TransactionsReview(string idOfTransaction, string period, string page)
        {
            if (idOfTransaction != null)
            {
                var transaction = this._context.Transactions
                    .Where(x => x.Id == idOfTransaction)
                    .FirstOrDefault();

                if (transaction == null)
                {
                    return this.Redirect("/home/error404");
                }

                var sender = this._context.BankAccounts
                    .Where(x => x.Id == transaction.SenderBankAccId)
                    .FirstOrDefault();

                var receiver = this._context.BankAccounts
                    .Where(x => x.Id == transaction.ReceiverBankAccId)
                    .FirstOrDefault();

                using var transactionDb = this._context.Database.BeginTransaction();
                try
                {
                    sender.Balance += transaction.Ammount;

                    receiver.Balance -= transaction.Ammount;

                    this._context.Transactions.Remove(transaction);

                    this._context.SaveChanges();

                    transactionDb.Commit();
                }

                catch (Exception Ex)
                {
                    transactionDb.Rollback();
                    return this.Redirect("/home/error404");
                }

            }

            string[] periodAcceptedValues = { "today", "7days", "30days", "all" };

            if (!periodAcceptedValues.Contains(period) && period != null)
            {
                return this.Redirect("/home/error404");
            }

            this.TempData["period"] = period;

            var selectedTransactions = this._getTransactions.GetAdminTransactionsForPeriod(this._context, period);

            var adminDetails = this.AdminFinder();

            var singlePageLength = 10;

            var pageToInt = page == null ? 1 : int.Parse(page);

            var totalPages = Math.Ceiling((decimal)selectedTransactions.Count / singlePageLength) == 0 ?
                1
                : Math.Ceiling((decimal)selectedTransactions.Count / singlePageLength);

            if (pageToInt > totalPages || pageToInt < 1)
            {
                return this.Redirect("/home/error404");
            }

            var pagedTransactions = selectedTransactions
                .Skip(singlePageLength * (pageToInt - 1))
                .Take(singlePageLength)
                .ToList();

            var transactionAllModel = new TransactionAllViewModel
            {
                UserNavbarModel = new UserNavbarViewModel
                {
                    FirstName = adminDetails.FirstName,
                    LastName = adminDetails.LastName,
                    PhotoUrl = adminDetails.PhotoUrl,
                },
                Transactions = pagedTransactions,
                PeriodReturn = period,
                TotalPages = totalPages,
                CurrentPage = pageToInt,
            };


            return this.View(transactionAllModel);
        }

        public IActionResult AddNews()
        {
            var adminDetails = this.AdminFinder();

            var errorReceivedData = (bool?)this.TempData["IsError"] ?? false;

            var messagesReceivedData = ((string[])this.TempData["Messages"]) ?? new string[0];

            var model = new AddNewsModel
            {
                UserNavbarModel = new UserNavbarViewModel
                {
                    FirstName = adminDetails.FirstName,
                    LastName = adminDetails.LastName,
                    PhotoUrl = adminDetails.PhotoUrl,
                },
                SuccessOrError = new SuccessOrErrorMessageForPartialViewModel(),
            };

            model.SuccessOrError.IsError = errorReceivedData;

            model.SuccessOrError.AllMessages = messagesReceivedData;

            return this.View(model);
        }

        [HttpPost]
        public IActionResult AddNews(AddNewsModel model)
        {

            if (this.ModelState.IsValid)
            {
                var newsToAdd = new News
                {
                    Date = DateTime.UtcNow,
                    PhotoUrl = model.ImgUrl,
                    Title = model.Title,
                    Description = model.Description
                };

                this._context.News.Add(newsToAdd);

                this._context.SaveChanges();

                this.TempData["Messages"] = new string[] { "News successfully added" };
            }

            else
            {
                this.TempData["IsError"] = true;

                this.TempData["Messages"] = this._collector.ErrorCollector(this.ModelState).ToArray();
            }

            return this.Redirect("/admin/adminhome/addnews");
        }

        public IActionResult CustomerService()
        {
            var adminDetails = this.AdminFinder();

            var model = new UserNavbarViewModel
            {
                FirstName = adminDetails.FirstName,
                LastName = adminDetails.LastName,
                PhotoUrl = adminDetails.PhotoUrl
            };

            return this.View(model);
        }

        public IActionResult SingleChat(string usernameforgroup, bool isadmin)
        {
            var model = new SingleChatModel
            {
                IsAdmin = isadmin,
                UserNameForGroup = usernameforgroup,
                Username = "Administrator"
            };

            return this.View(model);
        }

        private ApplicationUser AdminFinder()
        {
            return this._context.Users
                .Where(x => x.UserName == this.User.Identity.Name)
                .FirstOrDefault();
        }
    }
}
