using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QienUrenMVC.Models;

namespace QienUrenMVC.Repositories
{
    public interface IClientRepository
    {
        Task<List<ClientModel>> Get(string searchString);
        Task<ClientModel> GetById(int id);
        Task<ClientModel> CreateNewClient(ClientModel clientModel);
        Task DeleteClient(int id);
        Task<ClientModel> Update(ClientModel client);
        Task<string> GetNameByID(int id);
    }
}