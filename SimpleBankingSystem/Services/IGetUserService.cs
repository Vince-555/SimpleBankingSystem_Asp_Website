using Microsoft.AspNetCore.Identity;
using SimpleBankingSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Services
{
    public interface IGetUserService
    {
        public ApplicationUser GetUser(UserManager<ApplicationUser> manager, string userName);
    }
}
