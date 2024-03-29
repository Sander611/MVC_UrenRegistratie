﻿using Microsoft.AspNetCore.Mvc;
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

        //get all clients in search
        public async Task<List<ClientModel>> Get(string searchString)
        {
            var allClients = new List<ClientModel>();
            foreach (var client in await context.Clients.Where(x => x.CompanyName.Contains(searchString) || x.ClientName1.Contains(searchString) || x.ClientName2.Contains(searchString) || searchString == null).ToListAsync())

                allClients.Add(new ClientModel
                {
                    ClientId = client.ClientId,
                    ClientEmail1 = client.ClientEmail1,
                    ClientEmail2 = client.ClientEmail2,
                    ClientName1 = client.ClientName1,
                    ClientName2 = client.ClientName2,
                    CompanyName = client.CompanyName
                });

            return allClients;
        }
        //find clients by id
        public async Task<ClientModel> GetById(int id)
        {
            
            var oneClient = await context.Clients.FindAsync(id);
            return new ClientModel
            {
                ClientId = oneClient.ClientId,
                ClientEmail1 = oneClient.ClientEmail1,
                ClientEmail2 = oneClient.ClientEmail2,
                ClientName1 = oneClient.ClientName1,
                ClientName2 = oneClient.ClientName2,
                CompanyName = oneClient.CompanyName
            };
        }
        //get client name
        public  async Task<string> GetNameByID(int id)
        {
            var oneClient = await context.Clients.FindAsync(id);
            string CompanyName = oneClient.CompanyName;
            return CompanyName;
        }
        //create a new client
        public async Task<ClientModel> CreateNewClient(ClientModel clientModel)
        {
            Client newClient = new Client()
            {
                ClientEmail1 = clientModel.ClientEmail1,
                ClientEmail2 = clientModel.ClientEmail2,
                ClientName1 = clientModel.ClientName1,
                ClientName2 = clientModel.ClientName2,
                CompanyName = clientModel.CompanyName
            };
                context.Clients.Add(newClient);
                await context.SaveChangesAsync();

            return clientModel;
        }

        //delete client
        public async Task DeleteClient(int id)
        {
            var client = await context.Clients.FindAsync(id);
            context.Clients.Remove(client);
            await context.SaveChangesAsync();
        }

        //update client info
        public async Task<ClientModel> Update(ClientModel client)
        {
            Client clientEntity = context.Clients.Single(p => p.ClientId == client.ClientId);
            clientEntity.ClientEmail1 = client.ClientEmail1;
            clientEntity.ClientEmail2 = client.ClientEmail2;
            clientEntity.ClientName1 = client.ClientName1;
            clientEntity.ClientName2 = client.ClientName2;
            clientEntity.CompanyName = client.CompanyName;
            await context.SaveChangesAsync();
            return client;
        }
    }
}
