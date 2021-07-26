using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Data.Models
{
    public class UserAddress
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [MaxLength(AddressMaxLength)]
        public string StreetAddress { get; set; }

        [MaxLength(GeneralInputFieldMaxLenght)] 
        public string City { get; set; }

        [MaxLength(GeneralInputFieldMaxLenght)]
        public string Country { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [InverseProperty("Address")]
        public ApplicationUser User { get; set; }
    }
}
