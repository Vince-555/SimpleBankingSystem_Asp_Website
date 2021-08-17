using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystem.Data;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Controllers
{
    [Authorize(Roles = "user")]
    public class ProfileController:Controller
    {
        private readonly SBSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IGetUserService _getUserService;
        private readonly IErrorCollector _collector;

        public ProfileController(SBSDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signinManager,
            IGetUserService getUserService,
            IErrorCollector collector)
        {
            this._context = context;
            this._userManager = userManager;
            this._getUserService = getUserService;
            this._collector = collector;
            this._signInManager = signinManager;
        }

        public IActionResult Profile()
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var errorReceivedData = (bool?)this.TempData["IsError"] ?? false;

            var messagesReceivedData = ((string[])this.TempData["Messages"]) ?? new string[0];

            // var address = this._context.UserAddresses.Where(x => x.UserId == user.Id).FirstOrDefault();
            //ef fails to load address otherwise even though include is used???

            if (user.Address==null)
            {
                var newAddress = new UserAddress()
                {
                    StreetAddress = String.Empty,
                    City = String.Empty,
                    Country = String.Empty,
                    UserId = user.Id,
                    User = user,
                };

                this._context.UserAddresses.Add(newAddress);

                this._context.SaveChanges();

                user.Address = this._context.UserAddresses.Where(x => x.UserId == user.Id).FirstOrDefault();
            }

            var profileModel = this.ProfileModelFiller();

            profileModel.SuccessOrError.IsError = errorReceivedData;

            profileModel.SuccessOrError.AllMessages = messagesReceivedData;

            return this.View(profileModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserSettings(ChangeUserSettingsModel model)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            int newSettings = 0;

            if (this.ModelState.IsValid)
            {
                if (model.FirstName != null)
                {
                    user.FirstName = model.FirstName;

                    newSettings++;
                }

                if (model.LastName != null)
                {
                    user.LastName = model.LastName;

                    newSettings++;
                }

                if (model.Email != null)
                {
                    user.Email = model.Email;

                    user.UserName = model.Email;

                    await this._userManager.UpdateAsync(user);

                    await _signInManager.SignOutAsync();

                    return RedirectToAction("Login","User");
                }

                if (newSettings == 0)
                {
                    this.TempData["IsError"] = true;

                    this.TempData["Messages"] = new string[] { "No settings have been updated" };   
                }

                else
                {
                    await this._userManager.UpdateAsync(user);

                    this.TempData["Messages"] = new string[] { "User settings updated successfully" };
                    
                }
            }
            else
            {
                this.TempData["IsError"] = true;

                this.TempData["Messages"] = this._collector.ErrorCollector(this.ModelState).ToArray();
               
            }

            return this.RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePhoto(string photoUrl)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            Uri uriResult;

            bool result = Uri.TryCreate(photoUrl, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if(result)
            {
                user.PhotoUrl = photoUrl;

                await this._userManager.UpdateAsync(user);
            }

            return this.RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            if (this.ModelState.IsValid)
            {
                var result = await this._userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    await this._userManager.UpdateAsync(user);

                    this.TempData["Messages"] = new string[] { "Password successfully changed" };
                }

                else
                {
                    this.TempData["IsError"] = true;

                    this.TempData["Messages"] = new string[] { "Incorrect current password" };
                }
            }

            else
            {
                this.TempData["IsError"] = true;

                this.TempData["Messages"] = this._collector.ErrorCollector(this.ModelState).ToArray();  
            }

            return this.RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult ChangeAddress(ChangeAddressModel model)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            if(this.ModelState.IsValid)
            {
                if (ListOfAllCountries.Names.Contains(model.Country))
                {
                    user.Address.City = model.City;

                    user.Address.StreetAddress = model.Address;

                    user.Address.Country = model.Country;

                    this._context.SaveChanges();

                    this.TempData["Messages"] = new string[] { "Address successfully updated" };
                }

                else
                {
                    this.TempData["IsError"] = true;

                    this.TempData["Messages"] = new string[] { "Enter a correct country" };
                }
            }

            else
            {
                this.TempData["IsError"] = true;

                this.TempData["Messages"] = this._collector.ErrorCollector(this.ModelState).ToArray();    
            }

            return this.RedirectToAction("Profile");
        }

        private ProfileViewModel ProfileModelFiller()
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var profileModel = new ProfileViewModel()
            {
                UserNavbarModel = new UserNavbarViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhotoUrl = user.PhotoUrl,
                },
                Address = user.Address.StreetAddress,
                City = user.Address.City,
                Country = user.Address.Country,
                Email = user.Email,
                SuccessOrError = new SuccessOrErrorMessageForPartialViewModel(),
            };

            return profileModel;
        }
    }
}
