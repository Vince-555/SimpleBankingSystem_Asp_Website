using System.ComponentModel.DataAnnotations;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Models
{
    public class ForgotPasswordFormModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [RegularExpression(EmailRegEx, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }
}
