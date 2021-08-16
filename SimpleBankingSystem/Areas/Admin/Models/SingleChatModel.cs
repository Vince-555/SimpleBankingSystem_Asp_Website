using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Areas.Admin.Models
{
    public class SingleChatModel
    {
        public string UserNameForGroup { get; set; }

        public bool IsAdmin { get; set; }

        public string Username { get; set; }
    }
}
