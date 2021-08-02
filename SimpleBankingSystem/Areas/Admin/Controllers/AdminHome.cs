using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBankingSystem.Areas.Admin.Models;
using SimpleBankingSystem.Data;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class AdminHomeController :Controller
    {
        private readonly SBSDbContext _context;
        private readonly IErrorCollector _collector;

        public AdminHomeController(SBSDbContext context, 
            IErrorCollector collector)
        {
            this._context = context;
            this._collector = collector;
        }

        public IActionResult TransactionsReview(string idOfTransaction, string period)
        {
            if (idOfTransaction != null)
            {
                var transaction = this._context.Transactions
                    .Where(x => x.Id == idOfTransaction)
                    .FirstOrDefault();

                if (transaction == null)
                {
                    return this.Redirect("home/error404/error404");
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

            var allTransactions = this._context.Transactions
                .Include(x => x.Sender)
                .ThenInclude(x => x.User)
                .Include(x => x.Receiver)
                .ThenInclude(x => x.User)
                .Select(x=>new TransactionModel 
                { 
                    TransactionId = x.Id,
                    Date = x.Date,
                    Description = x.Description.Length > 20 ? x.Description.Substring(0, 20) + "..." : x.Description,
                    Ammount = x.Ammount.ToString("G", CultureInfo.InvariantCulture),
                    From = x.Sender.User.FirstName + " " + x.Sender.User.LastName,
                    To = x.Receiver.User.FirstName + " " + x.Receiver.User.LastName,
                })
                .ToList();

            DateTime receivedDateTimePeriod;

            switch (period)
            {
                case "today":
                    receivedDateTimePeriod = new DateTime(DateTime.UtcNow.Year,
                        DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 1);
                    break;
                case "7days":
                    receivedDateTimePeriod = DateTime.UtcNow.AddDays(-7d);
                    break;
                case "30days":
                    receivedDateTimePeriod = DateTime.UtcNow.AddDays(-30d);
                    break;
                default:
                    receivedDateTimePeriod = DateTime.MinValue;
                    break;
            }

            var selectedTransactions = allTransactions
                .Where(x => DateTime.Compare(x.Date, receivedDateTimePeriod) >= 0)
                .OrderByDescending(x => x.Date)
                .ToList();

            Dictionary<string, string> selectedPeriodReturn = new Dictionary<string, string>
            {
                {"today",String.Empty},
                {"7days",String.Empty},
                {"30days",String.Empty},
                {"all",String.Empty}
            };

            if (period != null)
            {
                selectedPeriodReturn[period] = "selected=\"\"";
            }

            var adminDetails = this.AdminFinder();

            var transactionAllModel = new TransactionAllViewModel
            {
                UserNavbarModel = new UserNavbarViewModel
                {
                    FirstName = adminDetails.FirstName,
                    LastName = adminDetails.LastName,
                    PhotoUrl = adminDetails.PhotoUrl,
                },
                Transactions = selectedTransactions,
                selectedPeriodReturnForView = selectedPeriodReturn
            };


            return this.View(transactionAllModel);
        }

        public IActionResult AddNews()
        {
            var adminDetails = this.AdminFinder();

            var model = new AddNewsModel
            {
                UserNavbarModel = new UserNavbarViewModel
                {
                    FirstName = adminDetails.FirstName,
                    LastName = adminDetails.LastName,
                    PhotoUrl = adminDetails.PhotoUrl,
                },
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult AddNews(AddNewsModel model)
        {
            AddNewsModel newsModel;

            SuccessOrErrorMessageForPartialViewModel successOrError;

            var adminDetails = this.AdminFinder();

            var userNavbarModel = new UserNavbarViewModel
            {
                FirstName = adminDetails.FirstName,
                LastName = adminDetails.LastName,
                PhotoUrl = adminDetails.PhotoUrl,
            };

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

                newsModel = new AddNewsModel
                {
                    UserNavbarModel = userNavbarModel,
                    SuccessOrError = new SuccessOrErrorMessageForPartialViewModel
                    {
                        AllMessages = new List<string>
                        {
                            "News successfully added"
                        }
                    },
                };

            }

            else
            {
                successOrError = new SuccessOrErrorMessageForPartialViewModel
                {
                    IsError = true,
                    AllMessages = this._collector.ErrorCollector(this.ModelState),
                };

                newsModel = new AddNewsModel
                {
                    SuccessOrError = successOrError,
                    UserNavbarModel = userNavbarModel,
                };
            }

           return this.View("addNews", newsModel);

        }

        private ApplicationUser AdminFinder()
        {
            return this._context.Users
                .Where(x => x.UserName == this.User.Identity.Name)
                .FirstOrDefault();
        }
    }
}
