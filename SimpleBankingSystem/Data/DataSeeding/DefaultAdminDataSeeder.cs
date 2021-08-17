using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SimpleBankingSystem.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Data.DataSeeding
{
    public class DefaultAdminDataSeeder
    {
        private readonly SBSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DefaultAdminDataSeeder(SBSDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task SeedAdmin()
        {
            var user = new ApplicationUser
            {
                Email = "admin@sbs.com",
                EmailConfirmed = true,
                FirstName = "User",
                LastName = "Administrator",
                Id = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                NormalizedEmail = "ADMIN@SBS.COM",
                UserName = "admin@sbs.com",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedUserName = "ADMIN@SBS.COM",
                BankAccount = new BankAccount
                {
                    UserId = "1"
                }
            };

            var roleStore = new RoleStore<IdentityRole>(_context);

            if (!_context.Users.Any(x => x.UserName == user.UserName))
            {
               await this._userManager.CreateAsync(user, "admin1234");
            }

            if (!_context.Roles.Any(x => x.Name == "admin"))
            {
                await this._roleManager.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "ADMIN" });
                var userToAdd = await this._userManager.FindByEmailAsync("admin@sbs.com");
                await this._userManager.AddToRoleAsync(userToAdd, "admin");
            }

            if (!_context.Roles.Any(x => x.Name == "user"))
            {
                await this._roleManager.CreateAsync(new IdentityRole { Name = "user", NormalizedName = "USER" });
            }

            await _context.SaveChangesAsync();
        }
    }
}
