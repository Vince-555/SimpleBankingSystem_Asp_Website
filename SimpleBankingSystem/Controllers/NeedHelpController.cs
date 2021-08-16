using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Controllers
{
    public class NeedHelpController :Controller
    {
        public IActionResult Chat()
        {
            return this.View();
        }

        public IActionResult Test()
        {
            return this.View();
        }
    }
}
