using System.ComponentModel.DataAnnotations;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Models
{
    public class ChangeUserSettingsModel
    {
        [MinLength(UserNamesMinLength,ErrorMessage ="First name is too short")]
        [MaxLength(UserNamesMaxLength,ErrorMessage ="First name is too long")]
        [RegularExpression(@"\b([A-ZÀ-ÿ][-,a-z. ']+[ ]*)+",ErrorMessage ="Please enter a correct name")]
        public string FirstName { get; set; }

        [MinLength(UserNamesMinLength, ErrorMessage = "Last name is too short")]
        [MaxLength(UserNamesMaxLength, ErrorMessage = "Last name is too long")]
        [RegularExpression(@"\b([A-ZÀ-ÿ][-,a-z. ']+[ ]*)+", ErrorMessage = "Please enter a correct name")]
        public string LastName { get; set; }

        [RegularExpression(EmailRegEx, ErrorMessage = "Please enter a valid email address")]
        [EmailAddress(ErrorMessage ="Please enter a valid email")]
        public string Email { get; set; }

        [MinLength(UserNamesMinLength, ErrorMessage = "Username is too short")]
        [MaxLength(UserNamesMaxLength, ErrorMessage = "Username is too long")]
        public string Username { get; set; }
    }
}
