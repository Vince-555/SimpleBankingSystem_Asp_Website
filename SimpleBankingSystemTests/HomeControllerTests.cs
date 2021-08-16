using MyTested.AspNetCore.Mvc;
using SimpleBankingSystem.Controllers;
using SimpleBankingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBankingSystemTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexShouldReturnDefaultView()
        => MyMvc
        .Controller<HomeController>()
        .Calling(c => c.Index())
        .ShouldReturn()
        .View();


    }
}
