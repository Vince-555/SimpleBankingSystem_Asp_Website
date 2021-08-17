using System.ComponentModel.DataAnnotations;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Please enter your current password")]
        [DataType(DataType.Password)]
        [MinLength(MinPasswordLength, ErrorMessage = "Current password should be at least 6 characters long")]
        [RegularExpression(PaswordRegEx, ErrorMessage = "Please enter a valid current password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter a new password")]
        [DataType(DataType.Password)]
        [MinLength(MinPasswordLength, ErrorMessage = "Your new password should be at least 6 characters long")]
        [RegularExpression(PaswordRegEx, ErrorMessage = "Please enter a valid new password with at least one digit")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please repeat new password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match.")]
        public string RepeatPassword { get; set; }
    }
}
