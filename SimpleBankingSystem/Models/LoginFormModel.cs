using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Models
{
    public class LoginFormModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [RegularExpression(EmailRegEx, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [MinLength(MinPasswordLength, ErrorMessage = "Your password should be at least 6 characters long")]
        [RegularExpression(PaswordRegEx,ErrorMessage = "Please enter a valid password with at least one digit")]
        public string Password { get; set; }

        public string RememberMe { get; set; }
    }
}
