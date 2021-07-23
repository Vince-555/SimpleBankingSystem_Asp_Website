using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Models
{
    public class TransactionAllViewModel
    {
        public UserNavbarViewModel UserNavbarModel { get; set; }

        public ICollection<TransactionModel> Transactions { get; set; }

        public Dictionary<string, string> selectedPeriodReturnForView { get; set; }
    }
}
