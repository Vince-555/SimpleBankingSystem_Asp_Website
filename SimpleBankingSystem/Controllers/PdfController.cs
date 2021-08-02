using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystem.Data;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBankingSystem.Controllers
{
    [Authorize]
    public class PdfController :Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SBSDbContext _context;
        private readonly IGetUserService _getUserService;
        private readonly IGetUserTransactions _getUserTransactions;
        private readonly IGetAdminTransaction _getAdminTransaction;

        public PdfController(UserManager<ApplicationUser> userManager,
            IGetUserService getUserService,
            IGetUserTransactions getUserTransactions,
            IGetAdminTransaction getAdminTransaction,
            SBSDbContext context)
        {
            this._userManager = userManager;
            this._context = context;
            this._getUserService = getUserService;
            this._getUserTransactions = getUserTransactions;
            this._getAdminTransaction = getAdminTransaction;
        }

        public IActionResult Print()
        {
            var period = this.TempData["period"] as string;

            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var periodForModel = period ?? "all";

            PdfPrintModel pdfModel;

            if (this.User.IsInRole("admin"))
            {
                var transactions = this.GetAdminTransactionsForPeriod(period);

                pdfModel = new PdfPrintModel
                {
                    UserFullName = "Site Administrator",
                    UserEmail = user.Email,
                    Transactions = transactions,
                    Period = periodForModel,
                };
            }

            else
            {
                var selectedTransactions = this._getUserTransactions.GetUserTransactions(user, period);

                pdfModel = new PdfPrintModel
                {
                    UserFullName = user.FirstName + " " + user.LastName,
                    UserEmail = user.Email,
                    Transactions = selectedTransactions,
                    Period = periodForModel,
                };
            }

            return this.View(pdfModel);
        }

        public List<TransactionModel> GetAdminTransactionsForPeriod(string period)
        {
            var adminTransactions = this._getAdminTransaction.GetAdminTransactions(this._context);
                 
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

            var selectedTransactions = adminTransactions
                .Where(x => DateTime.Compare(x.Date, receivedDateTimePeriod) >= 0)
                .OrderByDescending(x => x.Date)
                .ToList();

            return selectedTransactions;
        }
    }
}

