using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest_QienUren.FakeRepositories;
using QienUrenMVC.Repositories;
using QienUrenMVC.Models;
using QienUrenMVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UnitTest_QienUren
{
    [TestClass]
    public class TestEmailController
    {
        [TestMethod]
        public void SendEmail_Should_Return_Redirect()
        {
            //arrange
            var controller = new EmailController(new FakeAccountRepository());

            //Act
            var result = controller.SendEmail("hallo", "1");

            //Assert
            Assert.IsInstanceOfType(result, typeof(Task<RedirectToRouteResult>));
        }

        [TestMethod]
        public void Send_Should_Return_AccountModel_Collection()
        {
            var controller = new EmailController(new FakeAccountRepository());

            List<AccountModel> accounts = new List<AccountModel>();
            var result = controller.Send(accounts, "message");

            Assert.IsInstanceOfType(result, typeof(Task));
        }


    }
}
