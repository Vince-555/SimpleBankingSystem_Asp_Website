using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Models
{
    public class TransactionAllViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoUrl { get; set; }

        public ICollection<TransactionModel> Transactions { get; set; }

        public Dictionary<string, string> selectedPeriodReturnForView { get; set; }
    }
}
