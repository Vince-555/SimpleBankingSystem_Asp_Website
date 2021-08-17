using System.Collections.Generic;

namespace SimpleBankingSystem.Models
{
    public class NewsPageModel
    {
        public bool IsSingle { get; set; }

        public UserNavbarViewModel UserNavbarModel {get; set;}

        public ICollection<SingleNewsModel> News { get; set; }


    }
}
