using Microsoft.EntityFrameworkCore;
using SimpleBankingSystem.Data;
using SimpleBankingSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Services
{
    public class GetAdminTransactionsService : IGetAdminTransaction
    {
        public List<TransactionModel> GetAdminTransactions(SBSDbContext context)
        {
            var allTransactions = context.Transactions
                .Include(x => x.Sender)
                .ThenInclude(x => x.User)
                .Include(x => x.Receiver)
                .ThenInclude(x => x.User)
                .Select(x => new TransactionModel
                {
                    Type = "In",
                    TransactionId = x.Id,
                    Date = x.Date,
                    Description = x.Description.Length > 20 ? x.Description.Substring(0, 20) + "..." : x.Description,
                    Ammount = x.Ammount.ToString("G", CultureInfo.InvariantCulture),
                    From = x.Sender.User.FirstName + " " + x.Sender.User.LastName,
                    To = x.Receiver.User.FirstName + " " + x.Receiver.User.LastName,
                })
                .ToList();

            return allTransactions;
        }
    }
}
