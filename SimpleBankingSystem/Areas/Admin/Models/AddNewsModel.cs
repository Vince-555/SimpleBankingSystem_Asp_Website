using SimpleBankingSystem.Models;
using System.ComponentModel.DataAnnotations;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Areas.Admin.Models
{
    public class AddNewsModel
    {
        [Required(ErrorMessage = "Please enter a tittle")]
        [MinLength(NewsTittleMinLength, ErrorMessage = "Tittle is too short")]
        [MaxLength(NewsTittleMaxLenght, ErrorMessage = "Tittle is too long")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter an Image Url")]
        [Url(ErrorMessage = "Please enter a valid Image Url")]
        public string ImgUrl { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        [MinLength(NewsMinDescription, ErrorMessage = "Description is too short")]
        public string Description { get; set; }

        public UserNavbarViewModel UserNavbarModel { get; set; }

        public SuccessOrErrorMessageForPartialViewModel SuccessOrError { get; set; }


    }
}
