using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Models
{
    public class IndexDashboardViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoUrl { get; set; }

        public decimal MontlyEarnings { get; set; }

        public decimal Balanace { get; set; }

        public int ActiveCards { get; set; }

        public string Iban { get; set; }

        public SuccessOrErrorMessageForPartialViewModel SuccessOrError { get; set; }
    }
}
