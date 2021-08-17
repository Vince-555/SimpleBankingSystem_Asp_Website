using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankingSystem.Data.Models
{
    public class BankAccount
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ConcurrencyCheck]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = Convert.ToDecimal(new Random().Next(9500, 235850)); //pre set balance for educational purposes

        public string Iban { get; set; } =
            "SBS2021" + Guid.NewGuid().ToString("N").Substring(0, 15).ToUpper();

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [InverseProperty("Sender")]
        public ICollection<Transaction> SentTransactions { get; set; }

        [InverseProperty("Receiver")]
        public ICollection<Transaction> ReceivedTransactions { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
