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

namespace SimpleBankingSystem.Controllers
{
    [Authorize]
    public class ProfileController:Controller
    {
        private readonly SBSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetUserService _getUserService;

        public ProfileController(SBSDbContext context, UserManager<ApplicationUser> userManager,
            IGetUserService getUserService)
        {
            this._context = context;
            this._userManager = userManager;
            this._getUserService = getUserService;
        }

        public IActionResult Profile()
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);
           
            var address = this._context.UserAddresses.Where(x => x.UserId == user.Id).FirstOrDefault();
            //ef fails to load address otherwise even though include is used???

            if(address==null)
            {
                var newAddress = new UserAddress()
                {
                    StreetAddress = String.Empty,
                    City = String.Empty,
                    Country = String.Empty,
                    UserId = user.Id,
                };

                this._context.UserAddresses.Add(newAddress);

                this._context.SaveChanges();

                address = this._context.UserAddresses.Where(x => x.UserId == user.Id).FirstOrDefault();
            }

            var profileModel = new ProfileViewModel()
            {
                UserNavbarModel = new UserNavbarViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhotoUrl = user.PhotoUrl,
                },
                Address = address.StreetAddress,
                City = address.City,
                Country = address.Country,
                Email = user.Email,
                Username = user.UserName,
            };

            return this.View(profileModel);
        }
    }
}
