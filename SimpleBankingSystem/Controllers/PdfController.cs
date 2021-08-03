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
        private readonly IGetTransactions _getTransactions;

        public PdfController(UserManager<ApplicationUser> userManager,
            IGetUserService getUserService,
            IGetTransactions getTransactions,
            SBSDbContext context)
        {
            this._userManager = userManager;
            this._context = context;
            this._getUserService = getUserService;
            this._getTransactions = getTransactions;
        }

        public IActionResult Print()
        {
            var period = this.TempData["period"] as string;

            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var periodForModel = period ?? "all";

            PdfPrintModel pdfModel;

            if (this.User.IsInRole("admin"))
            {
                var transactions = this._getTransactions.GetAdminTransactionsForPeriod(this._context, period);

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
                var selectedTransactions = this._getTransactions.GetUserTransactionsForPeriod(user, period);

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

    }
}

