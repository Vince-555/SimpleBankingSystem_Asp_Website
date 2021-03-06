using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleBankingSystem.Data.Models;
using System.Linq;

namespace SimpleBankingSystem.Services
{
    public class GetUserService : IGetUserService
    {
        public ApplicationUser GetUser(UserManager<ApplicationUser> manager, string userName)
        {
            return manager.Users
                .Where(x => x.UserName == userName)
                .Include(x=>x.Address)
                .Include(x => x.BankAccount)
                .ThenInclude(x => x.SentTransactions)
                .ThenInclude(x=>x.Receiver)
                .ThenInclude(x=>x.User)
                .Include(x=>x.BankAccount)
                .ThenInclude(x=>x.ReceivedTransactions)
                .ThenInclude(x=>x.Sender)
                .ThenInclude(x=>x.User)
                .Include(x => x.BankAccount)
                .ThenInclude(x => x.Cards)
                .FirstOrDefault();
        }
    }
}
