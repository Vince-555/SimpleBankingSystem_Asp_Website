using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystem.Data;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;
using System;
using System.Linq;

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

        public IActionResult All(string period, string page)
        {
            string[] periodAcceptedValues = { "today", "7days", "30days", "all" };

            if (!periodAcceptedValues.Contains(period) && period!=null)
            {
                return this.Redirect("/home/error404");
            }

            this.TempData["period"] = period; //used in pdf controller !!!

            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var selectedTransactions = this._getTransactions.GetUserTransactionsForPeriod(user, period);

            var singlePageLength = 10;

            var pageToInt = page==null ? 1 : int.Parse(page);

            var totalPages = Math.Ceiling((decimal)selectedTransactions.Count / singlePageLength) == 0 ?
                1
                : Math.Ceiling((decimal)selectedTransactions.Count / singlePageLength);

            if (pageToInt>totalPages || pageToInt<1)
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
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhotoUrl = user.PhotoUrl,
                },                
                Transactions = pagedTransactions,
                PeriodReturn = period,
                TotalPages = totalPages,
                CurrentPage = pageToInt,
            };


            return this.View(transactionAllModel);
        }
    }
}
