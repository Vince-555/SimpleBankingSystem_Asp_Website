using System;

namespace SimpleBankingSystem.Models
{
    public class TransactionModel
    {
        public string Type { get; set; }

        public string Ammount { get; set; }

        public DateTime Date { get; set; }

        public string TransactionId { get; set; }

        public string Description { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
}
