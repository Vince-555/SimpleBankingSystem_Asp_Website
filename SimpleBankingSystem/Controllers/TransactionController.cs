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
    [Authorize(Roles = "user")]
    public class TransactionController:Controller
    {
        private readonly SBSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetUserService _getUserService;
        private readonly IGetTransactions _getTransactions;

        public TransactionController(SBSDbContext context, UserManager<ApplicationUser> userManager,
            IGetUserService getUserService,
            IGetTransactions getTransactions)
        {
            this._context = context;
            this._userManager = userManager;
            this._getUserService = getUserService;
            this._getTransactions = getTransactions;
        }

        public IActionResult All(string period)
        {
            string[] periodAcceptedValues = { "today", "7days", "30days", "all" };

            if (!periodAcceptedValues.Contains(period) && period!=null)
            {
                return this.View("/home/error404");
            }

            this.TempData["period"] = period;

            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var selectedTransactions = this._getTransactions.GetUserTransactionsForPeriod(user, period);

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
