using Microsoft.EntityFrameworkCore;
using SimpleBankingSystem.Data;
using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Services
{
    public class GetTransactionsService : IGetTransactions
    {
       public List<TransactionModel> GetUserTransactionsForPeriod(ApplicationUser user, string period)
        {
            var userReceivedTransactions = user.BankAccount.ReceivedTransactions
                 .Select(x => new TransactionModel
                 {
                     Type = "In",
                     Date = x.Date,
                     Description = x.Description.Length > 20 ? x.Description.Substring(0, 20) + "..." : x.Description,
                     Ammount = x.Ammount.ToString("G", CultureInfo.InvariantCulture),
                     TransactionId = x.Id.ToUpper(),
                     From = x.Sender.User.FirstName + " " + x.Sender.User.LastName,
                     To = "Your account"

                 }).ToList();

            var userSentTransactions = user.BankAccount.SentTransactions
                .Select(x => new TransactionModel
                {
                    Type = "Out",
                    Date = x.Date,
                    Description = x.Description.Length > 20 ? x.Description.Substring(0, 20) + "..." : x.Description,
                    Ammount = x.Ammount.ToString("G", CultureInfo.InvariantCulture),
                    TransactionId = x.Id.ToUpper(),
                    To = x.Receiver.User.FirstName + " " + x.Receiver.User.LastName,
                    From = "Your account"

                }).ToList();

            var userTransactionsCombined = userReceivedTransactions.Concat(userSentTransactions).ToList();

            return this.TransactionPeriodFilter(userTransactionsCombined, period);   
        }

        public List<TransactionModel> GetAdminTransactionsForPeriod(SBSDbContext context, string period)
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

            return this.TransactionPeriodFilter(allTransactions, period);
        }

        private List<TransactionModel> TransactionPeriodFilter (List<TransactionModel> transactions, string period)
        {
            DateTime receivedDateTimePeriod;

            switch (period)
            {
                case "today":
                    receivedDateTimePeriod = new DateTime(DateTime.UtcNow.Year,
                        DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 1);
                    break;
                case "7days":
                    receivedDateTimePeriod = DateTime.UtcNow.AddDays(-7d);
                    break;
                case "30days":
                    receivedDateTimePeriod = DateTime.UtcNow.AddDays(-30d);
                    break;
                default:
                    receivedDateTimePeriod = DateTime.MinValue;
                    break;
            }

            var selectedTransactions = transactions
                .Where(x => DateTime.Compare(x.Date, receivedDateTimePeriod) >= 0)
                .OrderByDescending(x => x.Date)
                .ToList();

            return selectedTransactions;
        }
    }
}
