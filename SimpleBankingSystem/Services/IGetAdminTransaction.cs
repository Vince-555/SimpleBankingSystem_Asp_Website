using SimpleBankingSystem.Data;
using SimpleBankingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Services
{
    public interface IGetAdminTransaction
    {
        public List<TransactionModel> GetAdminTransactions(SBSDbContext context);
    }
}
