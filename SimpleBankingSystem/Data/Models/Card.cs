using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Data.Models
{
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(GeneralInputFieldMaxLenght)]
        public string Type { get; set; }

        public bool IsBlocked { get; set; }

        [MaxLength(GeneralInputFieldMaxLenght)]
        public string CardName { get; set; }

        public DateTime ExpDate { get; set; }

        public string BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}
