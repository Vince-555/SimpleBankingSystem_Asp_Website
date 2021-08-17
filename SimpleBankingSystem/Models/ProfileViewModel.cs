
namespace SimpleBankingSystem.Models
{
    public class ProfileViewModel
    {
        public SuccessOrErrorMessageForPartialViewModel SuccessOrError { get; set; }

        public UserNavbarViewModel UserNavbarModel { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}
