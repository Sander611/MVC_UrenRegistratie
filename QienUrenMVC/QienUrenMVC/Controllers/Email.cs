using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMVC.Controllers
{
    public class EmailController : Controller
    {
        private readonly IAccountRepository accountRepo;
        public EmailController(IAccountRepository AccountRepo)
        {
            accountRepo = AccountRepo;
        }
        [HttpPost]
        public async Task<RedirectToRouteResult> SendEmail(string adminText, string Roles)
        {
            if (Roles == "1")
            {
                List<AccountModel> medewerkers = await accountRepo.GetAllAccountsWithoutString();
                await Send(medewerkers, adminText);
            }
            else if (Roles == "2")
            {
                List<AccountModel> medewerkers = await accountRepo.GetAllTrainees();
                await Send(medewerkers, adminText);
            }
            else if(Roles == "3")
            {
                List<AccountModel> medewerkers = await accountRepo.GetAllQienMedewerkers();
                await Send(medewerkers, adminText);
            }
            else
            {
                List<AccountModel> medewerkers = await accountRepo.GetAllSeniors();
                await Send(medewerkers, adminText);
            }
            return RedirectToRoute(new { controller = "Admin", action = "Dashboard" });
        }
        public async Task Send(List<AccountModel> accountList, string messageAdmin)
        {
            foreach (var account in accountList)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("QienUrenRegistratie", "GroepTweeQien@gmail.com"));
                message.To.Add(new MailboxAddress($"{account.FirstName} {account.LastName}", account.Email));
                message.Subject = "Bericht van Admin";
                message.Body = new TextPart("plain")
                {
                    Text = $"Beste {account.FirstName} {account.LastName}," +
                        $" U heeft een bericht van admin onvangen. " +
                        $"Het bericht is " +
                        $"{messageAdmin}"
                };
                using (var smptcli = new SmtpClient())
                {
                    smptcli.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    smptcli.Connect("Smtp.gmail.com", 587, false);
                    smptcli.Authenticate("GroepTweeQien@gmail.com", "GroepQien2019!");
                    smptcli.Send(message);
                    smptcli.Disconnect(true);
                }
            }
        }
    }
}
