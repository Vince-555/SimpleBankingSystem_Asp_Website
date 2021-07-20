using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleBankingSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {        
            return this.View();
        }

        public IActionResult TestError()
        {
            SuccessOrErrorMessageForPartialViewModel model = null;
            return this.View(model);
           
        }
    }
}
