using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Data.Models
{
    public class Transaction
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 7);

        [Column(TypeName = "decimal(18,2)")]
        public decimal Ammount { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string SenderId { get; set; }

        public User Sender { get; set; }

        public string ReceiverId { get; set; }

        public User Receiver { get; set; }

    }
}
