using MyTested.AspNetCore.Mvc;
using SimpleBankingSystem.Controllers;
using SimpleBankingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SBSTests
{
    public class UserControllerTests
    {
        [Fact]
        public void LogintShouldReturnDefaultView()
        => MyMvc
        .Controller<UserController>()
        .Calling(c => c.Login())
        .ShouldReturn()
        .View(x => x.WithModelOfType<SuccessOrErrorMessageForPartialViewModel>());

        [Fact]
        public void LoginShouldRedirectIfCalledByLoggedAdmin()
            => MyMvc
            .Controller<UserController>()
            .WithUser(x => x.InRole("admin"))
            .Calling(x => x.Login())
            .ShouldReturn()
            .Redirect("/admin/adminhome/transactionsreview");

        [Fact]
        public void LoginShouldRedirectIfCalledByLoggedUser()
            => MyMvc
            .Controller<UserController>()
            .WithUser(x => x.InRole("user"))
            .Calling(x => x.Login())
            .ShouldReturn()
            .Redirect("/home/index");


        [Fact]
        public void RegisterShouldReturnDefaultView()
        => MyMvc
        .Controller<UserController>()
        .Calling(c => c.Register())
        .ShouldReturn()
        .View(x => x.WithModelOfType<SuccessOrErrorMessageForPartialViewModel>());

        [Fact]
        public void ForgotPasswordShouldReturnDefaultView()
        => MyMvc
        .Controller<UserController>()
        .Calling(c => c.ForgotPassword())
        .ShouldReturn()
        .View(x => x.WithModelOfType<SuccessOrErrorMessageForPartialViewModel>());



    }
}
