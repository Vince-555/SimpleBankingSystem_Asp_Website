using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Data.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required]
        [MaxLength(UserNamesMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserNamesMaxLength)]
        public string LastName { get; set; }

        public string PhotoUrl { get; set; } = DefaultUserPhotoLocation;

        [Required]
        public string BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }

        public string AddressId { get; set; }

        public Address Address { get; set; }




    }
}
