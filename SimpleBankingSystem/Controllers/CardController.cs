using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystem.Data;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Controllers
{
    [Authorize]
    public class CardController:Controller
    {
        private readonly SBSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetUserService _getUserService;

        public CardController(SBSDbContext context, UserManager<ApplicationUser> userManager,
            IGetUserService getUserService)
        {
            this._context = context;
            this._userManager = userManager;
            this._getUserService = getUserService;
        }

        public IActionResult AllCards(int block, int unblock, int remove)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            SuccessOrErrorMessageForPartialViewModel successOrError = new SuccessOrErrorMessageForPartialViewModel()
            {
                IsError = false,
                AllMessages = new List<string>(),
            };

            if(block!=0)
            {
                var card = user.BankAccount.Cards
                    .Where(x => x.Id == block)
                    .FirstOrDefault();

                if (card==null || card.IsBlocked)
                {
                    return null; //404 when page is available
                }

                card.IsBlocked = true;
                this._context.SaveChanges();
            }

            if (unblock != 0)
            {
                var card = user.BankAccount.Cards
                    .Where(x => x.Id == unblock)
                    .FirstOrDefault();

                if (card == null || !card.IsBlocked)
                {
                    return null; //404 when page is available
                }

                card.IsBlocked = false;
                this._context.SaveChanges();
            }

            if (remove != 0)
            {
                var card = user.BankAccount.Cards
                    .Where(x => x.Id == remove)
                    .FirstOrDefault();

                if (card == null)
                {
                    return null; //404 when page is available
                }

                this._context.Remove(card);
                this._context.SaveChanges();
            }

            var allCardsModel = this.AllCardsViewModelFiller(successOrError);

            return this.View(allCardsModel);
        }

        public IActionResult AddCard(string card, string cardName)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            SuccessOrErrorMessageForPartialViewModel successOrError = new SuccessOrErrorMessageForPartialViewModel()
            {
                IsError = false,
                AllMessages = new List<string>(),
            };

            var allCardsModel = this.AllCardsViewModelFiller(successOrError);

            if (card!= "visa" && card!="mastercard")
            {
                return null; //return 404 when page is available
            }

            if (cardName.Length > GeneralInputFieldMaxLenght)
            {
                successOrError.IsError = true;
                successOrError.AllMessages.Add("Card name is too long");
                allCardsModel.SuccessOrError = successOrError;
                return this.View("AllCards",allCardsModel);
            }

            if(user.BankAccount.Cards.Count==5)
            {
                successOrError.IsError = true;
                successOrError.AllMessages.Add("You have reached max number of cards");
                allCardsModel.SuccessOrError = successOrError;
                return this.View("AllCards", allCardsModel);
            }

            var newCard = new Card()
            {
                ExpDate = DateTime.UtcNow.AddDays(730d),
                BankAccountId = user.BankAccountId,
                CardName = cardName,
                Type = card,
                IsBlocked = false,
            };

            this._context.Cards.Add(newCard);

            this._context.SaveChanges();

            successOrError.AllMessages.Add("Your card has been added successfully. " +
                "Please collect it at your local branch");

            allCardsModel = this.AllCardsViewModelFiller(successOrError);

            return this.View("AllCards", allCardsModel);
        }

        private AllCardsViewModel AllCardsViewModelFiller (SuccessOrErrorMessageForPartialViewModel successOrError)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);
            var userValidCards = user
                .BankAccount
                .Cards
                .Where(x => DateTime.Compare(DateTime.UtcNow, x.ExpDate) <= 0)
                .Select(x => new CardViewModel
                {
                    Id = x.Id,
                    ExpDate = x.ExpDate,
                    Status = x.IsBlocked == true ? "Blocked" : "Active",
                    Type = x.Type,
                    Name = x.CardName
                }).ToList();
           
            var model = new AllCardsViewModel()
            {
                UserNavBarModel = new UserNavbarViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhotoUrl = user.PhotoUrl,
                },
                SuccessOrError = successOrError,
                AllCards = userValidCards
            };

            return model;
        }
    }
}
