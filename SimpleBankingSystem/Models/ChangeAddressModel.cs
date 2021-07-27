using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Models
{
    public class ChangeAddressModel
    {
        [Required(ErrorMessage = "Please enter a city")]
        [MinLength(CityMinLength, ErrorMessage = "City name is too short")]
        [MaxLength(GeneralInputFieldMaxLenght, ErrorMessage = "City name is too long")]
        [RegularExpression(CityRegeEx, ErrorMessage = "Please enter a valid city name")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter an address")]
        [MinLength(AddressMinLength, ErrorMessage = "Address is too short")]
        [MaxLength(AddressMaxLength, ErrorMessage = "Address is too long")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter a country")]
        public string Country { get; set; }
    }
}
