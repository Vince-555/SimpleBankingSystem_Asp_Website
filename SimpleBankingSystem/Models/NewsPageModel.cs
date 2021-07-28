using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Models
{
    public class NewsPageModel
    {
        public bool IsSingle { get; set; }

        public UserNavbarViewModel UserNavbarModel {get; set;}

        public ICollection<SingleNewsModel> News { get; set; }


    }
}
