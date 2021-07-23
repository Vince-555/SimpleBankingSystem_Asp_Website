using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class TransactionController:Controller
    {
        private readonly SBSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetUserService _getUserService;

        public TransactionController(SBSDbContext context, UserManager<ApplicationUser> userManager,
            IGetUserService getUserService)
        {
            this._context = context;
            this._userManager = userManager;
            this._getUserService = getUserService;
        }

        [Authorize]
        public IActionResult All(string period)
        {
            string[] periodAcceptedValues = { "today", "7days", "30days", "all" };

            if (!periodAcceptedValues.Contains(period) && period!=null)
            {
                return null; //redirect to 404 page when available
            }

            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var userReceivedTransactions = user.BankAccount.ReceivedTransactions
                .Select(x => new TransactionModel
                {
                    Type = "In",
                    Date = x.Date,
                    Description = x.Description.Length > 20 ? x.Description.Substring(0, 20) + "..." : x.Description,
                    Ammount = x.Ammount.ToString("G", CultureInfo.InvariantCulture),
                    TransactionId = x.Id.ToUpper(),
                    From = x.Sender.User.FirstName + " " + x.Sender.User.LastName,
                    To = "Your account"

                }).ToList();

            var userSentTransactions = user.BankAccount.SentTransactions
                .Select(x => new TransactionModel
                {
                    Type = "Out",
                    Date = x.Date,
                    Description = x.Description.Length > 20 ? x.Description.Substring(0, 20) + "..." : x.Description,
                    Ammount = x.Ammount.ToString("G", CultureInfo.InvariantCulture),
                    TransactionId = x.Id.ToUpper(),
                    To = x.Receiver.User.FirstName + " " + x.Receiver.User.LastName,
                    From = "Your account"

                }).ToList();

            var userTransactionsCombined = userReceivedTransactions.Concat(userSentTransactions).ToList();

            DateTime receivedDateTimePeriod;

            switch (period)
            {
                case "today":
                    receivedDateTimePeriod = new DateTime(DateTime.UtcNow.Year,
                        DateTime.UtcNow.Month,DateTime.UtcNow.Day,0,0,1);
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

            var selectedTransactions = userTransactionsCombined
                .Where(x => DateTime.Compare(x.Date, receivedDateTimePeriod) >= 0)
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

            var transactionAllModel = new TransactionAllViewModel
            {
                UserNavbarModel = new UserNavbarViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhotoUrl = user.PhotoUrl,
                },                
                Transactions = selectedTransactions,
                selectedPeriodReturnForView = selectedPeriodReturn
            };


            return this.View(transactionAllModel);
        }
    }
}
