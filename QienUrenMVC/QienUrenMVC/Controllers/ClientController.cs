using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllClients(string searchString, string errorText)
        {
            ViewBag.error = errorText;
            List<ClientModel> result = await clientRepo.Get(searchString);
            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ClientDetails(int id)
        {
            ClientModel result = await clientRepo.GetById(id);
            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateClient()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateClient(ClientModel newClient)
        {
            ClientModel client = await clientRepo.CreateNewClient(newClient);
            return RedirectToRoute(new { controller = "Client", action = "GetAllClients" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateClient(int id)
        {
            var result = await clientRepo.GetById(id);
            return View(result);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public async Task<RedirectToRouteResult> DeleteClient(int id)
        {
            var client = await clientRepo.GetById(id);
            HoursPerDayModel formsById = await hoursperdayRepo.GetAllFormsByClientId(id);

            if (client == null || formsById != null)
            {
                if(formsById != null)
                {
                    return RedirectToRoute(new { controller = "Client", action = "GetAllClients", errorText = "Deze werkgever kan niet worden verwijderd. Er zijn nog urenformulieren gekoppeld aan deze werkgever." });
                    //throw new Exception("Cannot delete the client because it has forms assigned to it.");
                }
                else
                {
                    
                    throw new Exception("Cannot delete the client because it doesn't exist.");
                }
                
            }

            await clientRepo.DeleteClient(id);
            return RedirectToRoute(new { controller = "Client", action = "GetAllClients" });
        }

        [Authorize(Roles = "Admin")]
        public async Task SendEMail(bool keuring, int id)
        {
            AccountModel medewerkerInfo = await accountRepo.GetAccountByFormId(id);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("QienUrenRegistratie", "GroepTweeQien@gmail.com"));
            message.To.Add(new MailboxAddress($"{medewerkerInfo.FirstName} {medewerkerInfo.LastName}", medewerkerInfo.Email));
            if (keuring == true)
            {
                message.Subject = "Uren zijn goedgekeurd";
                message.Body = new TextPart("plain")
                {
                    Text = $"Beste {medewerkerInfo.FirstName} {medewerkerInfo.LastName}" +
                    $" Uw uren waren goedgekeurd!"
                };
            }
            else
            {
                message.Subject = "Uren zijn afgekeurd";
                message.Body = new TextPart("plain")
                {
                    Text = $"Beste {medewerkerInfo.FirstName} {medewerkerInfo.LastName}" +
                    $" Uw uren waren afgekeurd!"
                };
            }
            using (var smptcli = new SmtpClient())
            {
                smptcli.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smptcli.Connect("Smtp.gmail.com", 587, false);
                smptcli.Authenticate("GroepTweeQien@gmail.com", "GroepQien2019!");
                smptcli.Send(message);
                smptcli.Disconnect(true);
            }
        }


        /// De methodes hieronder kunnen door NIET-identity users benaderd worden.


        public async Task<IActionResult> ControlerenClient(int formId, string accountId, string fullName, string month, string year, int state, Guid token)
        {
            HoursFormModel hoursForm = await hoursformRepo.GetFormById(formId);
            if (hoursForm.Verification_code == token)
            {
                
                ViewBag.TotalHours = hoursForm.TotalHours;
                ViewBag.TotalSick = hoursForm.TotalSick;
                ViewBag.TotalOver = hoursForm.TotalOver;
                ViewBag.TotalLeave = hoursForm.TotalLeave;
                ViewBag.TotalOther = hoursForm.TotalOther;
                ViewBag.TotalTraining = hoursForm.TotalTraining;

                ViewBag.formId = formId;
                ViewBag.accountId = accountId;
                ViewBag.fullName = fullName;
                ViewBag.month = month;
                ViewBag.year = year;
                ViewBag.status = state;

                HoursFormModel formInfo = await hoursformRepo.GetFormById(formId);

                ViewBag.textClient = formInfo.CommentClient;
            }
            List<HoursPerDayModel> formsForId = await hoursperdayRepo.GetAllDaysForForm(formId);

            return View(formsForId);
        }

        [HttpPost]
        public async Task<IActionResult> CheckControleren([FromQuery]bool keuring, int id, string adminText, string clientText)
        {

            if (keuring == true)
            {
                await hoursformRepo.ChangeState(1, id, clientText, adminText);
            }
            else if (keuring == false)
            {
                await hoursformRepo.ChangeState(2, id, clientText, adminText);
            }
            await SendEMail(keuring, id);

            return RedirectToRoute(new
            {
                controller = "Client",
                action = "checkedPage",
                keuringBool = keuring
            });
        }

        
        public IActionResult checkedPage(bool keuringBool)
        {
            if (keuringBool)
            {
                ViewBag.keuring = "goedgekeurd!";
            }
            else
            {
                ViewBag.keuring = "afgekeurd!";
            }

            return View();
        }
    }
}