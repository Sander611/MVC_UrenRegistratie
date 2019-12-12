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
        public Task<ClientModel> CreateNewClient(ClientModel clientModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteClient(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ClientModel>> Get(string searchString)
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNameByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ClientModel> Update(ClientModel client)
        {
            throw new NotImplementedException();
        }
    }
}
