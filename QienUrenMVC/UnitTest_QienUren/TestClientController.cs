using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QienUrenMVC.Controllers;
using QienUrenMVC.Data;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest_QienUren
{
    [TestClass]
    public class TestClientController
    {
        [TestMethod]
        public async Task ClientDetailsReturnsIActionResult()
        {
            int testSessionId = 1;
            var mockRepoClient = new Mock<IClientRepository>();
            var mockRepoAccount = new Mock<IAccountRepository>();
            var mockRepoHoursForm = new Mock<IHoursFormRepository>();
            var mockRepoHoursPerDay = new Mock<IHoursPerDayRepository>();
            mockRepoClient.Setup(repo => repo.GetById(testSessionId))
                .Returns(Task.FromResult((GetTestClients().FirstOrDefault(s => s.ClientId == testSessionId))));
            var controller = new ClientController(mockRepoAccount.Object, mockRepoClient.Object, mockRepoHoursForm.Object, mockRepoHoursPerDay.Object);

            var result = await controller.ClientDetails(testSessionId);
            Assert.IsInstanceOfType(result, typeof(IActionResult));;
        }
        [TestMethod]
        public async Task CreateClient_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            var mockRepoClient = new Mock<IClientRepository>();
            var mockRepoAccount = new Mock<IAccountRepository>();
            var mockRepoHoursForm = new Mock<IHoursFormRepository>();
            var mockRepoHoursPerDay = new Mock<IHoursPerDayRepository>();
            var controller = new ClientController(mockRepoAccount.Object, mockRepoClient.Object, mockRepoHoursForm.Object, mockRepoHoursPerDay.Object);
            controller.ModelState.AddModelError("CompanyName", "Required");
            
            var newClient_No_CompanyName = new ClientModel()
            {

                ClientId = 1,
                ClientName1 = "Peter Peter",
                ClientName2 = "Jan Jan",
                ClientEmail1 = "test@test.com",
                ClientEmail2 = "test1@test.com"
            };
            mockRepoClient.Setup(repo => repo.CreateNewClient(newClient_No_CompanyName)).Returns(Task.FromResult(GetTestClient()));
            controller.ModelState.AddModelError("CompanyName", "Required");

            var result = await controller.CreateClient(newClient_No_CompanyName);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public async Task CreateClient_ReturnsARedirectAndAddsClient_WhenModelStateIsValid()
        {
            // Arrange
            var mockRepoClient = new Mock<IClientRepository>();
            var mockRepoAccount = new Mock<IAccountRepository>();
            var mockRepoHoursForm = new Mock<IHoursFormRepository>();
            var mockRepoHoursPerDay = new Mock<IHoursPerDayRepository>();
            mockRepoClient.Setup(repo => repo.CreateNewClient(GetTestClient())).Returns(Task.FromResult(GetTestClient()));
            var controller = new ClientController(mockRepoAccount.Object, mockRepoClient.Object, mockRepoHoursForm.Object, mockRepoHoursPerDay.Object);
            var newClient = new ClientModel()
            {
                CompanyName = "Macaw",
                ClientId = 1,
                ClientName1 = "Peter Peter",
                ClientName2 = "Jan Jan",
                ClientEmail1 = "test@test.com",
                ClientEmail2 = "test1@test.com"
            };

            var result = await controller.CreateClient(newClient);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
        [TestMethod]
        public async Task UpdateClient_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            var mockRepoClient = new Mock<IClientRepository>();
            var mockRepoAccount = new Mock<IAccountRepository>();
            var mockRepoHoursForm = new Mock<IHoursFormRepository>();
            var mockRepoHoursPerDay = new Mock<IHoursPerDayRepository>();
            var controller = new ClientController(mockRepoAccount.Object, mockRepoClient.Object, mockRepoHoursForm.Object, mockRepoHoursPerDay.Object);
            controller.ModelState.AddModelError("CompanyName", "Required");

            var newClient_No_CompanyName = new ClientModel()
            {

                ClientId = 1,
                ClientName1 = "Peter Peter",
                ClientName2 = "Jan Jan",
                ClientEmail1 = "test@test.com",
                ClientEmail2 = "test1@test.com"
            };
            mockRepoClient.Setup(repo => repo.Update(newClient_No_CompanyName)).Returns(Task.FromResult(GetTestClient()));
            controller.ModelState.AddModelError("CompanyName", "Required");

            var result = await controller.UpdateClient(newClient_No_CompanyName);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public async Task UpdateClient_ReturnsARedirectAndAddsClient_WhenModelStateIsValid()
        {
            // Arrange
            var mockRepoClient = new Mock<IClientRepository>();
            var mockRepoAccount = new Mock<IAccountRepository>();
            var mockRepoHoursForm = new Mock<IHoursFormRepository>();
            var mockRepoHoursPerDay = new Mock<IHoursPerDayRepository>();
            mockRepoClient.Setup(repo => repo.Update(GetTestClient())).Returns(Task.FromResult(GetTestClient()));
            var controller = new ClientController(mockRepoAccount.Object, mockRepoClient.Object, mockRepoHoursForm.Object, mockRepoHoursPerDay.Object);
            var newClient = new ClientModel()
            {
                CompanyName = "Macaw",
                ClientId = 1,
                ClientName1 = "Peter Peter",
                ClientName2 = "Jan Jan",
                ClientEmail1 = "test@test.com",
                ClientEmail2 = "test1@test.com"
            };

            var result = await controller.UpdateClient(newClient);

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
        [TestMethod]
        public async Task SendEmail_Returns_Task()
        {
            var mockRepoClient = new Mock<IClientRepository>();
            var mockRepoAccount = new Mock<IAccountRepository>();
            var mockRepoHoursForm = new Mock<IHoursFormRepository>();
            var mockRepoHoursPerDay = new Mock<IHoursPerDayRepository>();
            mockRepoClient.Setup(repo => repo.Update(GetTestClient())).Returns(Task.FromResult(GetTestClient()));
            var controller = new ClientController(mockRepoAccount.Object, mockRepoClient.Object, mockRepoHoursForm.Object, mockRepoHoursPerDay.Object);

            var result = controller.SendEMail(true, 1);

            Assert.IsInstanceOfType(result, typeof(Task));
        }

        private List<ClientModel> GetTestClients()
        {
            var clients = new List<ClientModel>();
            clients.Add(new ClientModel()
            {
                CompanyName = "Macaw",
                ClientId = 1,
                ClientName1 = "Peter Peter",
                ClientName2 = "Jan Jan",
                ClientEmail1 = "test@test.com",
                ClientEmail2 = "test1@test.com"
            });
            clients.Add(new ClientModel()
            {
                CompanyName = "Booking.com",
                ClientId = 2,
                ClientName1 = "Gert-Jan",
                ClientName2 = "Baas",
                ClientEmail1 = "gert-jan@booking.com",
                ClientEmail2 = "baas@test.com"
            });
            return clients;
        }
        private ClientModel GetTestClient()
        {
            var client = new ClientModel
                {
                ClientId = 1,
                ClientName1 = "Peter Peter",
                ClientName2 = "Jan Jan",
                ClientEmail1 = "test@test.com",
                ClientEmail2 = "test1@test.com"
            };
            return client;
        }
    }
}
