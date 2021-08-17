using System.Collections.Generic;

namespace SimpleBankingSystem.Models
{
    public class SuccessOrErrorMessageForPartialViewModel
    {
        public bool IsError { get; set; }

        public ICollection<string> AllMessages { get; set; }
    }
}
