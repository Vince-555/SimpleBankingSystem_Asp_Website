using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Data.Models
{
    public class Transaction
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 7);

        [Column(TypeName = "decimal(18,2)")]
        public decimal Ammount { get; set; }

        [MaxLength(TransactionDescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string SenderId { get; set; }

        public ApplicationUser Sender { get; set; }

        public string ReceiverId { get; set; }

        public ApplicationUser Receiver { get; set; }

    }
}
