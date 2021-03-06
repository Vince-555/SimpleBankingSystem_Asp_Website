using System;

namespace SimpleBankingSystem.Models
{
    public class CardViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public DateTime ExpDate { get; set; }
    }
}
