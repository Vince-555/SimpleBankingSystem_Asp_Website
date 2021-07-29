using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SimpleBankingSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Data.DataSeeding
{
    public class DefaultAdminDataSeeder
    {
        private SBSDbContext _context;

        public DefaultAdminDataSeeder(SBSDbContext context)
        {
            _context = context;
        }

        public async Task SeedAdmin()
        {
            var user = new ApplicationUser
            {
                Email = "admin@sbs.com",
                EmailConfirmed = true,
                FirstName = "User",
                LastName = "Administrator",
                Id = "1", //pontential issues
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
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "admin1234");
                user.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(_context);
                await userStore.CreateAsync(user);
            }

            if (!_context.Roles.Any(x => x.Name == "admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "ADMIN" });
                var userStore = new UserStore<ApplicationUser>(_context);
                await userStore.AddToRoleAsync(user, "admin");
            }

            await _context.SaveChangesAsync();
        }
    }
}
