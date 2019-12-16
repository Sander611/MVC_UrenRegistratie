using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_QienUren.FakeRepositories
{
    class FakeClientRepository : IClientRepository
    {

        private List<ClientModel> clients = new List<ClientModel>
        {
            new ClientModel
            {
                CompanyName = "Macaw",
                ClientId = 1,
                ClientName1 = "Peter",
                ClientName2 = "Jan Jan",
                ClientEmail1 = "test@test.com",
                ClientEmail2 = "test1@test.com"
            }

        };

        public Task<ClientModel> CreateNewClient(ClientModel clientModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteClient(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ClientModel>> Get(string searchString)
        {
            return clients;
        }

        public async Task<ClientModel> GetById(int id)
        {
            return clients[0];
        }

        public async Task<string> GetNameByID(int id)
        {
            return "Macaw";
        }

        public Task<ClientModel> Update(ClientModel client)
        {
            throw new NotImplementedException();
        }
    }
    
}
