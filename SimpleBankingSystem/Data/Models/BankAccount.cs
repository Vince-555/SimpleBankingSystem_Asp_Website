using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Data.Models
{
    public class BankAccount
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public decimal Balance { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
