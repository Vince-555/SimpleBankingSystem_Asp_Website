using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Services
{
    public interface IGetUserTransactions
    {
        public List<TransactionModel> GetUserTransactions(ApplicationUser user, string period);
    }
}
