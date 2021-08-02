using SimpleBankingSystem.Data.Models;
using SimpleBankingSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Services
{
    public class GetUserTransactionsService : IGetUserTransactions
    {
       public List<TransactionModel> GetUserTransactions(ApplicationUser user, string period)
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

            var selectedTransactions = userTransactionsCombined
                .Where(x => DateTime.Compare(x.Date, receivedDateTimePeriod) >= 0)
                .OrderByDescending(x => x.Date)
                .ToList();

            return selectedTransactions;
        }
    }
}
