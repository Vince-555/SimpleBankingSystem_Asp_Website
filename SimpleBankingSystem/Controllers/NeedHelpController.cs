using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleBankingSystem.Controllers
{
    [Authorize(Roles = "user")]
    public class NeedHelpController :Controller
    {
        public IActionResult Chat()
        {
            var model = this.User.Identity.Name;
            
            return this.View(nameof(Chat),model);
        }
    }
}
