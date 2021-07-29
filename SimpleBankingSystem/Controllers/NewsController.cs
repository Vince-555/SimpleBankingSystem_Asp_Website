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
    [Authorize(Roles = "user")]
    public class NewsController :Controller
    {
        private readonly SBSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetUserService _getUserService;

        public NewsController(SBSDbContext context, UserManager<ApplicationUser> userManager,
            IGetUserService getUserService)
        {
            this._context = context;
            this._userManager = userManager;
            this._getUserService = getUserService;
        }

        public IActionResult News(string readMoreId)
        {
            var user = this._getUserService.GetUser(this._userManager, this.User.Identity.Name);

            var userNavbarModel = new UserNavbarViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhotoUrl = user.PhotoUrl,
            };

            NewsPageModel modelToPass;

            if (readMoreId == null)
            {
                var latestNews = this._context.News
                    .OrderByDescending(x => x.Date)
                    .Take(4)
                    .Select(x => new SingleNewsModel
                    {
                        Id = x.Id,
                        Author = "Admin",
                        Date = x.Date.ToString("dd/MM/yyyy"),
                        Title = x.Title,
                        Description = x.Description.Length > 250 ? x.Description.Substring(0, 250) : x.Description,
                        PhotoUrl = x.PhotoUrl,
                    })
                    .ToList();

                modelToPass = new NewsPageModel
                {
                    UserNavbarModel = userNavbarModel,
                    News = latestNews
                };
            }

            else
            {
                var news = this._context.News
                    .Where(x => x.Id == int.Parse(readMoreId))
                    .Select(x => new SingleNewsModel
                    {
                        Author = "Admin",
                        Date = x.Date.ToString("dd/MM/yyyy"),
                        Title = x.Title,
                        Description = x.Description,
                        PhotoUrl = x.PhotoUrl,
                    })
                    .ToList();

                if (news.Count<1)
                {
                    return this.View("404");
                }

                modelToPass = new NewsPageModel
                {
                    IsSingle = true,
                    UserNavbarModel = userNavbarModel,
                    News = news,
                };
            }

            return this.View(modelToPass);
        }

    }
}
