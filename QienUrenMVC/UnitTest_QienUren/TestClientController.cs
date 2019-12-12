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
        public async Task ReturnsIActionResult()
        {
            int testSessionId = 1;
            var mockRepoClient = new Mock<IClientRepository>();
            var mockRepoAccount = new Mock<IAccountRepository>();
            var mockRepoHoursForm = new Mock<IHoursFormRepository>();
            var mockRepoHoursPerDay = new Mock<IHoursPerDayRepository>();
            mockRepoClient.Setup(repo => repo.GetById(testSessionId))
                .Returns(Task.FromResult((GetTestClients().FirstOrDefault(s => s.ClientId == testSessionId))));
            var controller = new ClientController(mockRepoAccount.Object, mockRepoClient.Object, mockRepoHoursForm.Object, mockRepoHoursPerDay.Object);

            // Act
            var result = await controller.ClientDetails(testSessionId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IActionResult));
            //var returnValue = Assert.IsType<ClientModel>(okResult.Value);
            //var idea = returnValue.FirstOrDefault();
            //Assert.Equals("One", idea.);
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
    }
}
