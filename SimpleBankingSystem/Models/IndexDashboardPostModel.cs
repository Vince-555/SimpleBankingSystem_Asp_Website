using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Models
{
    public class IndexDashboardPostModel
    {
        [Required(ErrorMessage = "Please enter receiver's name")]
        [MinLength(UserNamesMinLength,ErrorMessage = "Receiver's name must be at least two symbols long")]
        [MaxLength(UserNamesMaxLength, ErrorMessage = "Receiver's name is too long")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Please enter an Iban")]
        [MinLength(IbanMinLength, ErrorMessage = "Iban is too short")]
        [MaxLength(IbanMaxLength, ErrorMessage = "Iban is too long")]
        public string ReceiverIban { get; set; }

        [MaxLength(TransactionDescriptionMaxLength, ErrorMessage = "Descriptions is too long")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter an ammount")]
        [Range(1,999999,ErrorMessage = "Please enter correct ammount")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Please enter correct ammount")]
        public decimal Ammount { get; set; }
    }
}
