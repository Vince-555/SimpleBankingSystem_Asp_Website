using Microsoft.AspNetCore.Identity;
using SimpleBankingSystem.Data.Models;

namespace SimpleBankingSystem.Services
{
    public interface IGetUserService
    {
        public ApplicationUser GetUser(UserManager<ApplicationUser> manager, string userName);
    }
}
