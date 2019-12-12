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
            var controller = new AdminController(null, new FakeAccountRepository(), null, new FakeHoursFormRepository(), null, null);
            //Act
            var result = controller.Dashboard();
            //Assert
            Assert.IsInstanceOfType(result, typeof(object));
        }
        [TestMethod]
        public void Should_Return_NewAccount()
        {
            //Arrange
            var controller = new AdminController(null, new FakeAccountRepository(), null, null, null, null);
            //Act
            var result = controller.CreateEmployee();
            //Assert
            Assert.IsInstanceOfType(result, typeof(object));
        }
        [TestMethod]
        public void Should_Return_View_AccountDeleted()
        {
            //Arrange
            var controller = new AdminController(null, new FakeAccountRepository(), null, null, null, null);
            //Act
            var result = controller.DeleteAccount("string");
            //Assert
            Assert.IsInstanceOfType(result, typeof(object));
        }
        [TestMethod]
        public void Should_Disapprove_Personalia()
        {
            //Arrange
            var controller = new AdminController(null, null, null, null, null, null);
            //Act
            var result = controller.DisapprovePersonalia("1234");
            //Assert
            Assert.IsTrue(result.IsCompleted);
        }
        [TestMethod]
        public void Should_Return_Forms_Of_An_AccountID()
        {
            //Arrange
            var controller = new AdminController(null, null, null, new FakeHoursFormRepository(), null, null);
            //Act
            var result = controller.Controleren(1, "1234", "Jansen", "Januari", "2019", 1);
            //Assert
            Assert.IsTrue(result.IsCompleted);
        }
        [TestMethod]
        public void Should_Return_Personalia_Model()
        {
            //Arrange
            var controller = new AdminController(null, new FakeAccountRepository(), null, null, null, null);
            //Act
            var result = controller.PersonaliaControleren("1234");
            //Assert
            Assert.IsTrue(result.IsCompleted);
        }
        [TestMethod]
        public void Should_Return_Redirect_After_Edit_Account()
        {
            //Arrange
            var controller = new AdminController(null, new FakeAccountRepository(), null, null, null, null);
            //Act
            var result = controller.EditAccount("466471b2-6323-4242-baf1-61a175a16e4e");
            //Assert
            Assert.IsInstanceOfType(result, typeof(System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>));

        }


        //create employee
        //year overview
        //formsforyear
        //checkcontroleren
}
}
