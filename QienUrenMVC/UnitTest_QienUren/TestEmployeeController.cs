using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QienUrenMVC.Controllers;
using QienUrenMVC.Models;
using UnitTest_QienUren.FakeRepositories;

namespace UnitTest_QienUren
{
    [TestClass]
    public class TestEmployeeController
    {


        [TestMethod]
        public void Should_Return_Employee_Dashboard_View()
        {
            //Arrange
            var controller = new EmployeeController(null, new FakeAccountRepository(), null, new FakeHoursFormRepository(), null, null, null);

            //Act
            var result = controller.EmployeeDashboard("");

            //Assert
            Assert.IsInstanceOfType(result, typeof(object));
        }


        [TestMethod]
        public void Should_Return_Employee_Personalia_View()
        {
            //Arrange
            var controller = new EmployeeController(null, new FakeAccountRepository(), null, null, null, null, null);

            //Act
            var result = controller.EmployeePersonalia("");

            //Assert
            Assert.IsInstanceOfType(result, typeof(object));
        }

        [TestMethod]
        public void Should_Return_Employee_HoursRegistration_View()
        {
            //Arrange
            var controller = new EmployeeController(null, null, null, null, new FakeHoursPerDayRepository(), null, null);

            //Act

            List<HoursPerDayModel> model = new List<HoursPerDayModel>();
            var result = controller.HoursRegistration(model, true, 1);


            //Assert
            Assert.IsInstanceOfType(result, typeof(object));

        }


        [TestMethod]
        public async Task Personalia_Should_return_Correct_Model_Data()
        {
            
            var controller = new EmployeeController(null, new FakeAccountRepository(), null, null, null, null, null);
            string accountId = "1";
   
            //act
            var result = await controller.EmployeePersonalia(accountId);
            var viewResult = (Microsoft.AspNetCore.Mvc.ViewResult)result;
            var viewModel = (EmployeeUpdateAccountModel)viewResult.Model;

            //assert
            Assert.IsTrue(viewModel.FirstName == "Ron"  && viewModel.City == "Amersfoort");
        }

        [TestMethod]
        public async Task Dashboard_Should_return_Correct_Model_Data()
        {

            var controller = new EmployeeController(null, new FakeAccountRepository(), null, new FakeHoursFormRepository(), null, null, null);
            string accountId = "1";

            //act
            var result = await controller.EmployeeDashboard(accountId);
            var viewResult = (Microsoft.AspNetCore.Mvc.ViewResult)result;
            var viewModel = (EmployeeDashboardModel)viewResult.Model;

            //assert
            Assert.IsTrue(viewModel.Account.AccountId == "1" && viewModel.Account.IsActive == false);
        }


        [TestMethod]
        public async Task Personalia_Should_Have_Right_Model()
        {
            //arrange
            var controller = new EmployeeController(null, new FakeAccountRepository(), null, null, null, null, null);
            string accountId = "1";

            //act
            var result = await controller.EmployeePersonalia(accountId);
            var viewResult = (Microsoft.AspNetCore.Mvc.ViewResult)result;

            //assert
            Assert.IsInstanceOfType(viewResult.Model, typeof(EmployeeUpdateAccountModel));


        }


        [TestMethod]
        public async Task Employee_Personalia_Post_Should_Have_RedirectToRouteResult()
        {
            //arrange
            var controller = new EmployeeController(null, new FakeAccountRepository(), null, null, null, null, null);
            //act
            var result =  await controller.EmployeePersonalia(new EmployeeUpdateAccountModel());
            //assert
            Assert.IsInstanceOfType(result, typeof(Microsoft.AspNetCore.Mvc.RedirectToRouteResult));
        }
    }
}
