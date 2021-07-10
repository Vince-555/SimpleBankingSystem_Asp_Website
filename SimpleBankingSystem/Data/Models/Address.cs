using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Data.Models
{
    public class Address
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
