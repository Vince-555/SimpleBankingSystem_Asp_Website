using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SimpleBankingSystem.Data;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using SimpleBankingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBankingSystem.Controllers
{
    [Authorize(Roles = "user")]
    public class NewsController :Controller
    {
        private readonly SBSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetUserService _getUserService;
        private readonly IMemoryCache _cache;

        public NewsController(SBSDbContext context, UserManager<ApplicationUser> userManager,
            IGetUserService getUserService,
            IMemoryCache cache)
        {
            this._context = context;
            this._userManager = userManager;
            this._getUserService = getUserService;
            this._cache = cache;
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
                List<SingleNewsModel> latestNews;

                if(!this._cache.TryGetValue("latestNews",out latestNews))
                {
                    latestNews = this._context.News
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

                    this._cache.Set("latestNews", latestNews, TimeSpan.FromHours(1));
                }

                modelToPass = new NewsPageModel
                {
                    UserNavbarModel = userNavbarModel,
                    News = latestNews,
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
                    return this.Redirect("/home/error404");
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
