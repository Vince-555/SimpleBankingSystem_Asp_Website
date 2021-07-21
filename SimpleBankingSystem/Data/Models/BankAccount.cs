using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Data.Models
{
    public class BankAccount
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = Convert.ToDecimal(new Random().Next(9500, 235850)); //pre set balance for educational purposes

        public string Iban { get; set; } =
            "SBS2021" + Guid.NewGuid().ToString("N").Substring(0, 15).ToUpper();

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
