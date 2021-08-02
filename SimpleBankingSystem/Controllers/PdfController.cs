using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;


namespace SimpleBankingSystem.Controllers
{
    public class PdfController :Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetUserService _getUserService;
        private readonly IGetUserTransactions _getUserTransactions;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        public PdfController(UserManager<ApplicationUser> userManager,
            IGetUserService getUserService,
            IGetUserTransactions getUserTransactions,
            IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            this._userManager = userManager;
            this._getUserService = getUserService;
            this._getUserTransactions = getUserTransactions;
            this._razorViewToStringRenderer = razorViewToStringRenderer;
        }

        public IActionResult Print()
        {
            var period = this.TempData["period"] as string;

            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var selectedTransactions = this._getUserTransactions.GetUserTransactions(user, period);

            var pdfModel = new PdfPrintModel
            {
                UserFullName = user.FirstName + " " + user.LastName,
                UserEmail = user.Email,
                Transactions = selectedTransactions,
            };

            return this.View(pdfModel);
        }

    }
}

