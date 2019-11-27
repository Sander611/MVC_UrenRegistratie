using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;

namespace QienUrenMVC.Controllers
{
    public class ClientController : Controller
    {
        private readonly IAccountRepository accountRepo;
        private readonly IClientRepository clientRepo;
        private readonly IHoursFormRepository hoursformRepo;
        private readonly IHoursPerDayRepository hoursperdayRepo;

        public ClientController(
                                IAccountRepository AccountRepo,
                                IClientRepository ClientRepo,
                                IHoursFormRepository HoursFormRepo,
                                IHoursPerDayRepository HoursPerDayRepo)
        {
           
            accountRepo = AccountRepo;
            clientRepo = ClientRepo;
            hoursformRepo = HoursFormRepo;
            hoursperdayRepo = HoursPerDayRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            List<ClientModel> result = await clientRepo.Get();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> ClientDetails(int id)
        {
            ClientModel result = await clientRepo.GetById(id);
            return View(result);
        }

        [HttpGet]
        public IActionResult CreateClient()
        {
            return View();
        }

        [HttpPost]
        public async Task<ClientModel> CreateClient(ClientModel newClient)
        {
            ClientModel client = await clientRepo.CreateNewClient(newClient);
            return client;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateClient(int id)
        {
            var result = await clientRepo.GetById(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClient(ClientModel updatedClient)
        {
            if (ModelState.IsValid)
            {
                var client = await clientRepo.Update(updatedClient);
                return RedirectToRoute(new { controller = "Client", action = "GetAllClients" });
            }
            return View(updatedClient);
        }

        [HttpGet("{id}")]
        public async Task<RedirectToRouteResult> DeleteClient(int id)
        {
            var client = clientRepo.GetById(id);
            if (client == null)
            {
                throw new Exception("Cannot delete the client because it doesn't exist");
            }

            await clientRepo.DeleteClient(id);
            return RedirectToRoute(new { controller = "Client", action = "GetAllClients" });
        }
    }
}