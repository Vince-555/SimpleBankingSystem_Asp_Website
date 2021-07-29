using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBankingSystem.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminDashController :Controller
    {
        public IActionResult TransactionsReview()
        {
            return this.View();
        }

        public IActionResult AddNews()
        {
            return this.View();
        }
    }
}
