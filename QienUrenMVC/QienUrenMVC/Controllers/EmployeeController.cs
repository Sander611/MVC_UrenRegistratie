using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using Newtonsoft.Json;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;

namespace QienUrenMVC.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly ApiHelper helper;
        //public EmployeeController(ApiHelper helper)
        //{
        //    this.helper = helper;
        //}

        private readonly IAccountRepository accountRepo;
        private readonly IClientRepository clientRepo;
        private readonly IHoursFormRepository hoursformRepo;
        private readonly IHoursPerDayRepository hoursperdayRepo;

        public EmployeeController(
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
        public async Task<IActionResult> EmployeeDashboard(string accountId)
        {

            AccountModel result = await accountRepo.GetOneAccount(accountId);

            List<HoursFormModel> result1 = await hoursformRepo.getAllFormPerAccount(accountId);


            AccountModel accountInfo = new AccountModel()
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Address = result.Address,
                ZIP = result.ZIP,
                AccountId = result.AccountId,
                City = result.City
            };

            List<HoursFormModel> formsOverview = new List<HoursFormModel>();
            foreach (var form in result1)
            {
                formsOverview.Add(new HoursFormModel
                {
                    AccountId = form.AccountId,
                    FormId = form.FormId,
                    DateDue = form.DateDue,
                    Year = form.Year,
                    ProjectMonth = form.ProjectMonth
                });
            }
            EmployeeDashboardModel EDM = new EmployeeDashboardModel();
            EDM.Account = accountInfo;
            EDM.Forms = formsOverview;
            return View(EDM);
        }

        [HttpGet]
        public async Task<IActionResult> HoursRegistration(int formid)
        {
            List<HoursPerDayModel> formsForId = await hoursperdayRepo.GetAllDaysForForm(formid);
            var clientList = hoursperdayRepo.GetClientList();
            ViewBag.CompanyNames = clientList;
            return View(formsForId);
        }

        [HttpPost]
        public async Task<IActionResult> HoursRegistration(List<HoursPerDayModel> model, bool versturen)
        {
            var clientList = hoursperdayRepo.GetClientList();
            ViewBag.CompanyNames = clientList;

            if (ModelState.IsValid)
            {
                List<HoursPerDayModel> hpdModel = await hoursperdayRepo.Update(model);
                return View(model);
            }
            if(versturen == true)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("QienUrenRegistratie", "GroepTweeQien@gmail.com"));
                message.To.Add(new MailboxAddress($"{} {acc.LastName}", acc.Email));
                message.Subject = "New account was created";
                message.Body = new TextPart("plain")
                {
                    Text = "I am using MailKit"
                };
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("Smtp.gmail.com", 587, false);
                    client.Authenticate("GroepTweeQien@gmail.com", "Groep2Qien!");
                    client.Send(message);
                    client.Disconnect(true);
                }
                return RedirectToRoute(new { controller = "Admin", action = "AccountOverzicht" });
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> CreateFormForAccount(HoursFormModel hoursformModel)
        {

            hoursformModel.AccountId = "2216e96f-5268-4fd7-8179-515474fdac1c";
            hoursformModel.DateSend = DateTime.Now;
            hoursformModel.TotalHours = 100;
            hoursformModel.ProjectMonth = "november";
            hoursformModel.Year = 2019;
            hoursformModel.IsAcceptedClient = 1;
            hoursformModel.IsLocked = false;
            hoursformModel.CommentAdmin = "blabla";
            hoursformModel.CommentClient = "blabla";

            var result = await hoursformRepo.CreateNewForm(hoursformModel);

            return RedirectToRoute(new { controller = "Employee", action = "EmployeeDashboard" });
        }

        [HttpGet]
        public async Task<IActionResult> EmployeePersonalia(string accountId)
        {
            AccountModel accountUser = await accountRepo.GetOneAccount(accountId);
            return View(accountUser);
        }

        [HttpPost]
        public async Task<IActionResult> EmployeePersonalia(AccountModel updatedAccount)
        {

            if (ModelState.IsValid)
            {
                var existingAccount = await accountRepo.GetOneAccount(updatedAccount.AccountId);
                if (existingAccount == null)
                {
                    return View(updatedAccount);
                }
                AccountModel acc = await accountRepo.UpdateAccount(updatedAccount);

                return RedirectToRoute(new { controller = "Employee", action = "EmployeeDashboard" });
            }

            return View(updatedAccount);
        }
        public async Task<IActionResult> SturenNaarKlant(string email, string subject, string textMessage, string htmlMessage)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("QienUrenRegistratie", "GroepTweeQien@gmail.com"));

                message.To.Add(new MailboxAddress(email));

                message.Subject = subject;

                var builder = new BodyBuilder
                {
                    TextBody = textMessage,
                    HtmlBody = htmlMessage
                };

                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("Smtp.gmail.com", 587, false);
                    client.Authenticate("GroepTweeQien@gmail.com", "Groep2Qien!");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }

            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
            }
        }
    }
}
