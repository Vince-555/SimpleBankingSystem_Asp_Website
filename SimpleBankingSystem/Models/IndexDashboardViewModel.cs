using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Models
{
    public class IndexDashboardViewModel
    {
        public UserNavbarViewModel UserNavbarModel { get; set; }

        public decimal MontlyEarnings { get; set; }

        public decimal Balanace { get; set; }

        public int ActiveCards { get; set; }

        public string Iban { get; set; }

        public SuccessOrErrorMessageForPartialViewModel SuccessOrError { get; set; }
    }
}
