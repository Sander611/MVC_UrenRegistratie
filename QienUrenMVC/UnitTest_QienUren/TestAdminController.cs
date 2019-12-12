using Microsoft.VisualStudio.TestTools.UnitTesting;
using QienUrenMVC.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using UnitTest_QienUren.FakeRepositories;
using Microsoft.AspNetCore.Hosting;

namespace UnitTest_QienUren
{
    [TestClass]
    public class TestAdminController
    {

        [TestMethod]
        public void Should_Return_Admin_Dashboard()
        {
            //Arrange
            var controller = new AdminController(new FakeAccountRepository(), new FakeClientRepository(), new FakeHoursFormRepository(), new FakeHoursPerDayRepository());
            //Act
            var result = controller.Dashboard();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Should_Return_NewAccount()
        {
            //Arrange
            var controller = new AdminController(new FakeAccountRepository());
            //Act
            var result = controller.CreateEmployee();
            //Assert
            Assert.IsInstanceOfType(result, typeof(object));
        }
    }
}
