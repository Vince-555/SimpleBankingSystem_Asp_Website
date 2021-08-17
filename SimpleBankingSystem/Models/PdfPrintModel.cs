using System.Collections.Generic;

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
