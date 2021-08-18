using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SimpleBankingSystem.Data.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Data.DataSeeding
{
    public class DataSeederForPresentation
    {
        private readonly SBSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeederForPresentation (SBSDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public const string usernameEmail1 = "pesho@abv.bg";

        public const string usernameEmail2 = "ceko@abv.bg";

        public async Task SeedUsersTransactionsAndNews()
        {
            if(this._context.Users.Any(x=>x.UserName==usernameEmail1 || x.UserName==usernameEmail2))
            {
                return;
            }

            var user1 = new ApplicationUser
            {
                Email = usernameEmail1,
                EmailConfirmed = true,
                FirstName = "Pesho",
                LastName = "Peshev",
                LockoutEnabled = false,
                NormalizedEmail = usernameEmail1.ToUpper(),
                UserName = usernameEmail1,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedUserName = usernameEmail1.ToUpper(),
            };

            user1.BankAccount = new BankAccount
            {
                UserId = user1.Id
            };

            var user2 = new ApplicationUser
            {
                Email = usernameEmail2,
                EmailConfirmed = true,
                FirstName = "Ceko",
                LastName = "Cekov",
                LockoutEnabled = false,
                NormalizedEmail = usernameEmail2.ToUpper(),
                UserName = usernameEmail2,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedUserName = usernameEmail2.ToUpper(),
            };

            user2.BankAccount = new BankAccount
            {
                UserId = user1.Id
            };

            var roleStore = new RoleStore<IdentityRole>(_context);

            await this._userManager.CreateAsync(user1, "pesho1234");

            await this._userManager.CreateAsync(user2, "ceko1234");

            var userToAdd1 = await this._userManager.FindByEmailAsync(usernameEmail1);
            await this._userManager.AddToRoleAsync(userToAdd1, "user");

            var userToAdd2 = await this._userManager.FindByEmailAsync(usernameEmail2);
            await this._userManager.AddToRoleAsync(userToAdd2, "user");

            await _context.SaveChangesAsync();

            this.AddTransactionsToUsers(userToAdd1.BankAccountId, userToAdd2.BankAccountId);

            this.AddNews();
        }

        private void AddTransactionsToUsers (string userBankAccId1, string userBankAccId2)
        {
            DateTime[] dates =
            {
                new DateTime(2021,8,18),
                new DateTime(2021,8,18),
                new DateTime(2021,8,5),
                new DateTime(2021,8,2),
                new DateTime(2021,8,1),
                new DateTime(2021,7,30),
                new DateTime(2021,7,18),
                new DateTime(2021,7,10),
                new DateTime(2021,6,30),
                new DateTime(2021,5,30),
                new DateTime(2021,5,25),

            };

            var randomAmount = new Random();

            foreach(var date in dates)
            {
                this._context.Transactions.Add(new Transaction
                {
                    Ammount = randomAmount.Next(2,150),
                    Date = date,
                    Description = "Demo transaction",
                    ReceiverBankAccId = userBankAccId1,
                    SenderBankAccId = userBankAccId2,
                });;
            }

            this._context.SaveChanges();

        }

        private void AddNews()
        {
            string imageUrl = "https://static.independent.co.uk/s3fs-public/thumbnails/image/2017/07/11/11/harold-0.jpg?width=1200";

            string startupPath = Environment.CurrentDirectory;

            string loremIpsumText = File.ReadAllText(startupPath + @"\\LoremIpsum.txt");

            for (int i = 0; i < 4; i++)
            {
                this._context.News.Add(new News
                {
                    Title=$"News for presentation{i}",
                    Date = DateTime.UtcNow,
                    Description = loremIpsumText,
                    PhotoUrl = imageUrl,
                });

                this._context.SaveChanges();
            }
        }
    }
}
