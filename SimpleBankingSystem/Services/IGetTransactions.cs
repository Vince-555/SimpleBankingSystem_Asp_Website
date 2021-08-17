using SimpleBankingSystem.Data;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using System.Collections.Generic;

namespace SimpleBankingSystem.Services
{
    public interface IGetTransactions
    {
        public List<TransactionModel> GetUserTransactionsForPeriod(ApplicationUser user, string period);

        public List<TransactionModel> GetAdminTransactionsForPeriod(SBSDbContext context, string period);
    }
}
