using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QienUrenMVC.Data;
using QienUrenMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMVC.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext context;

        public ClientRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<List<ClientModel>> Get()
        {
            var allClients = new List<ClientModel>();
            foreach (var client in await context.Clients.ToListAsync())

                allClients.Add(new ClientModel
                {
                    ClientId = client.ClientId,
                    ClientEmail1 = client.ClientEmail1,
                    ClientEmail2 = client.ClientEmail2,
                    AccountId = client.AccountId,
                    ClientName1 = client.ClientName1,
                    ClientName2 = client.ClientName2,
                    CompanyName = client.CompanyName
                });

            return allClients;
        }
        public async Task<ClientModel> GetById(int id)
        {
            
            var oneClient = await context.Clients.FindAsync(id);
            return new ClientModel
            {
                ClientId = oneClient.ClientId,
                ClientEmail1 = oneClient.ClientEmail1,
                ClientEmail2 = oneClient.ClientEmail2,
                AccountId = oneClient.AccountId,
                ClientName1 = oneClient.ClientName1,
                ClientName2 = oneClient.ClientName2,
                CompanyName = oneClient.CompanyName
            };
        }

        public async Task<ClientModel> CreateNewClient(ClientModel clientModel)
        {
            Client newClient = new Client()
            {
                ClientEmail1 = clientModel.ClientEmail1,
                ClientEmail2 = clientModel.ClientEmail2,
                AccountId = clientModel.AccountId,
                ClientName1 = clientModel.ClientName1,
                ClientName2 = clientModel.ClientName2,
                CompanyName = clientModel.CompanyName
            };
                context.Clients.Add(newClient);
                await context.SaveChangesAsync();

            return clientModel;
        }
        public async Task DeleteClient(int id)
        {
            var client = await context.Clients.FindAsync(id);
            context.Clients.Remove(client);
            await context.SaveChangesAsync();
        }
        public async Task<ClientModel> Update(ClientModel client)
        {
            Client clientEntity = context.Clients.Single(p => p.ClientId == client.ClientId);
            clientEntity.ClientEmail1 = client.ClientEmail1;
            clientEntity.ClientEmail2 = client.ClientEmail2;
            clientEntity.AccountId = client.AccountId;
            clientEntity.ClientName1 = client.ClientName1;
            clientEntity.ClientName2 = client.ClientName2;
            clientEntity.CompanyName = client.CompanyName;
            await context.SaveChangesAsync();
            return client;
        }
        //public async Task<Client> VerifyEmail(string email)
        //{
        //    return await context.Clients.Find(email);
        //}
    }
}
