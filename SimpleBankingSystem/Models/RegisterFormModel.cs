using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Models
{
    public class RegisterFormModel
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [MinLength(UserNamesMinLength,ErrorMessage = "First name must be at least 2 symbols")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [MinLength(UserNamesMinLength,ErrorMessage = "Last name must be at least 2 symbols")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage ="Please enter a valid email address")]
        [RegularExpression(EmailRegEx,ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password,ErrorMessage = "Please enter a valid password")] //check if minlenght is auto applied
        public string Password { get; set; }

        [Required(ErrorMessage = "Please repeat password")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string PasswordRepeat { get; set; }
    }
}
