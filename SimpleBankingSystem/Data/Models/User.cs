using System;
using System.ComponentModel.DataAnnotations;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Data.Models
{
    public class User
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(UserNamesMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserNamesMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(EmailAddressMaxLength)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        [Required]
        public string BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }

        public string AddressId { get; set; }

        public Address Address { get; set; }




    }
}
