using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Models
{
    public class PdfPrintModel
    {
        public string UserFullName { get; set; }

        public string UserEmail { get; set; }

        public string Period { get; set; }

        public ICollection<TransactionModel> Transactions {get; set;}
    }
}
