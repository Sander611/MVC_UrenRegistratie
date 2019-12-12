using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using MimeKit;
using MailKit.Net.Smtp;
using QienUrenMVC.Data;
using Microsoft.AspNetCore.Identity;

namespace QienUrenMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;


        private readonly IAccountRepository accountRepo;
        private readonly IClientRepository clientRepo;
        private readonly IHoursFormRepository hoursformRepo;
        private readonly IHoursPerDayRepository hoursperdayRepo;
        private readonly IWebHostEnvironment hostingEnvironment;


        public EmployeeController(
                                IWebHostEnvironment hostingEnvironment,
                                IAccountRepository AccountRepo,
                                IClientRepository ClientRepo,
                                IHoursFormRepository HoursFormRepo,
                                IHoursPerDayRepository HoursPerDayRepo,
                                UserManager<UserIdentity> userManager,
                                SignInManager<UserIdentity> signinManager
                                )
        {
            this.hostingEnvironment = hostingEnvironment;
            accountRepo = AccountRepo;
            clientRepo = ClientRepo;
            hoursformRepo = HoursFormRepo;
            hoursperdayRepo = HoursPerDayRepo;
            _userManager = userManager;
            _signInManager = signinManager;
            
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeDashboard(string accountId)
        {
            //ophalen van account gegevens via accountId voor de model
            AccountModel result = await accountRepo.GetOneAccount(accountId);

            //collectie ophalen van hoursforms via accountId voor de model
            List<HoursFormModel> result1 = await hoursformRepo.getAllFormPerAccount(accountId);

            //nieuw account model aanmaken met de properties van de accountmodel die is binnengehaald
            AccountModel accountInfo = new AccountModel()
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Address = result.Address,
                ZIP = result.ZIP,
                AccountId = result.AccountId,
                City = result.City,
                ProfileImage = result.ProfileImage,
                IsTrainee = result.IsTrainee,
                IsQienEmployee = result.IsQienEmployee,
                IsSeniorDeveloper = result.IsSeniorDeveloper,

            };

            //nieuwe collectie met hourforms instantieren 
            List<HoursFormModel> formsOverview = new List<HoursFormModel>();

            //hourforms die zijn gevonden worden toegevoegd aan de collectie met hourforms
            foreach (var form in result1)
            {
                //voor elke hourform die is opgehaald een nieuwe hourform aanmaken met dezelfde properties
                formsOverview.Add(new HoursFormModel
                {
                    AccountId = form.AccountId,
                    FormId = form.FormId,
                    DateDue = form.DateDue,
                    Year = form.Year,
                    ProjectMonth = form.ProjectMonth,
                    IsAcceptedClient = form.IsAcceptedClient
                });
            }
            
            //het model voor de view bevat AccountModel en HoursFormModel
            EmployeeDashboardModel EDM = new EmployeeDashboardModel();

            //samenvoegen van de AccountModel en HourFormsModel in het model voor de view
            EDM.Account = accountInfo;
            EDM.Forms = formsOverview;

            return View(EDM);
        }

        [HttpGet]
        public async Task<IActionResult> HoursRegistration(int formid, int state)
        {
            //Ophalen van HoursPerDay via de HoursForm id uit de database
            List<HoursPerDayModel> formsForId = await hoursperdayRepo.GetAllDaysForForm(formid);

            //Ophalen van de huidige HoursForm om de info te laten zien
            HoursFormModel formInfo = await hoursformRepo.GetFormById(formid);

            //clientIdMonth ophalen voor de 1ste gevonden hoursform
            var clientIDmonth = formsForId[0].ClientId;

            //client naam ophalen via de clientIdMonth
            var ClientName = await clientRepo.GetNameByID(clientIDmonth.Value);

            //viewbag krijgt de info die is opgehaald om te laten zien in de view
            ViewBag.NameClient = ClientName;
            ViewBag.TotalHours = formInfo.TotalHours;
            ViewBag.TotalSick = formInfo.TotalSick;
            ViewBag.TotalOver = formInfo.TotalOver;
            ViewBag.TotalLeave = formInfo.TotalLeave;
            ViewBag.TotalOther = formInfo.TotalOther;
            ViewBag.TotalTraining = formInfo.TotalTraining;

            //alle clients ophalen als collectie
            var clientList = hoursperdayRepo.GetClientList();
            ViewBag.CompanyNames = clientList;

            ViewBag.FormId = formid;
            ViewBag.month = formsForId[0].Month;
            ViewBag.year = await hoursformRepo.GetYearOfForm(formid);
            ViewBag.status = state;

            //huidige account ophalen via de formid
            AccountModel account = await accountRepo.GetAccountByFormId(formid);
            ViewBag.accountId = account.AccountId;

            //als state gelijk is aan 4 betekent het afgekeurd. 
            if (state == 4)
            {
                //laat opmerking zien via viewbag
                ViewBag.textAdmin = formInfo.CommentAdmin;
                ViewBag.textClient = formInfo.CommentClient;
            }
            return View(formsForId);
        }

        //Opslaan en bewerken van een uren registratie. 
        //Email sturen na het afronden van je uren registratie
        [HttpPost]
        public async Task<IActionResult> HoursRegistration(List<HoursPerDayModel> model, bool versturen, int formid)
        {
            AccountModel medewerkerInfo = await accountRepo.GetAccountByFormId(formid);
            string Name = $"{medewerkerInfo.FirstName} {medewerkerInfo.LastName}";

            HoursFormModel hoursForm = await hoursformRepo.GetFormById(formid);
            hoursForm.DateSend = DateTime.Now;
            await hoursformRepo.EditForm(hoursForm);

            var clientList = hoursperdayRepo.GetClientList();

            ViewBag.CompanyNames = clientList;
            ViewBag.month = model[0].Month;
            ViewBag.year = await hoursformRepo.GetYearOfForm(model[0].FormId);
            ViewBag.FormId = formid;
            ViewBag.accountId = medewerkerInfo.AccountId;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<HoursPerDayModel> hpdModel = await hoursperdayRepo.Update(model);

                int totalHours = 0;
                int totalSick = 0;
                int totalOver = 0;
                int totalLeave = 0;
                int totalTraining = 0;
                int totalOther = 0;
                foreach(var perday in model)
                {
                    totalHours += perday.Hours;
                    totalSick += perday.IsSick;
                    totalOver += perday.OverTimeHours;
                    totalLeave += perday.IsLeave;
                    totalTraining += perday.Training;
                    totalOther += perday.Other;
                }

                await hoursformRepo.UpdateTotalHoursForm(model[0].FormId, totalHours, totalSick, totalOver, totalLeave, totalOther, totalTraining);
                ViewBag.status = 0;
                ViewBag.TotalHours = totalHours;
                ViewBag.TotalSick = totalSick;
                ViewBag.TotalOver = totalOver;
                ViewBag.TotalLeave = totalLeave;
                ViewBag.TotalOther = totalOther;
                ViewBag.TotalTraining = totalTraining;

     

                if (versturen == true)
                {

                    var clientIds = model.Select(m => m.ClientId).Distinct();
                    foreach (var client in clientIds)
                    {
                        var verificationCode = hoursForm.Verification_code;

                        var formId = formid;
                        var accountId = medewerkerInfo.AccountId;
                        var fullName = Name;
                        var month = hoursForm.ProjectMonth;
                        var year = hoursForm.Year;
                        var state = 5;
                        var token = verificationCode;

                        var callbackUrl = Url.Action(
                            "ControlerenClient",
                            "Client",
                            new {formId, accountId, fullName, month, year, state, token},
                            HttpContext.Request.Scheme,
                            HttpContext.Request.Host.Value
                            );

                        //var varifyUrl = $"https://localhost:44306/Client/ControlerenClient?formId={formid}&accountId={medewerkerInfo.AccountId}&fullName={Name}&month={hoursForm.ProjectMonth}&year={hoursForm.Year}&state=5&token={verificationCode}";
                        
                        
                        ClientModel client1 = await clientRepo.GetById(client.GetValueOrDefault());
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress("QienUrenRegistratie", "GroepTweeQien@gmail.com"));
                        message.To.Add(new MailboxAddress($"{client1.ClientName1}", client1.ClientEmail1));
                        message.Subject = "[Qien Urenregistratie-systeem] Controleer het urenformulier van " + fullName;
                        message.Body = new TextPart("plain")
                        {
                            Text = $"Het urenformulier voor de maand {month} is ingevuld door {fullName}. Klik op deze link om het urenformulier te keuren :\n{callbackUrl}"
                        };
                        using (var smptcli = new SmtpClient())
                        {
                            smptcli.ServerCertificateValidationCallback = (s, c, h, e) => true;
                            smptcli.Connect("Smtp.gmail.com", 587, false);
                            smptcli.Authenticate("GroepTweeQien@gmail.com", "GroepQien2019!");
                            smptcli.Send(message);
                            smptcli.Disconnect(true);

                            
                                          ////// hier moet de werkgever een mail krijgen met een link naar een pagina (deze link moet een unique token bevatten)
                                        ////// op die pagina moeten de uren van de werknemer te zien zijn en een opmerking veld en twee knoppen (afkeuren/goedkeuren)
                                        ////// wanneer de werkgever dit invuld en verstuurd zal de "IsClientAccepted" van dat form veranderen naar 1 (indien goedgekeurd) en anders naar 2 (afgekeurd).
                                        ////// tevens zal dan de "commentClient" geupdate worden met het commentaar van de werkgever
                            


                        }
                    }

                    await hoursformRepo.ChangeState(5, model[0].FormId, "", "");
                    ViewBag.status = 5;
                    return View(model);

                }
                return View(model);
        }

        //Uren formulier aanmaken voor de clientId
        [HttpPost]
        public async Task<IActionResult> CreateFormForAccount(HoursFormModel hoursformModel, int ClientId)
        {
            hoursformModel.AccountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            hoursformModel.TotalHours = 0;
            hoursformModel.ProjectMonth = "maart";
            hoursformModel.Year = 2018;
            hoursformModel.IsLocked = false;
            hoursformModel.CommentAdmin = "";
            hoursformModel.CommentClient = "";

            var result = await hoursformRepo.CreateNewForm(hoursformModel, ClientId);

            return RedirectToRoute(new { controller = "Employee", action = "EmployeeDashboard" });
        }

        //Personalia opvragen via de accountId voor het Model
        [HttpGet]
        public async Task<IActionResult> EmployeePersonalia(string accountId)
        {
            AccountModel accountUser = await accountRepo.GetOneAccount(accountId);
            EmployeeUpdateAccountModel tempacc = new EmployeeUpdateAccountModel()
            {
                AccountId = accountUser.AccountId,
                FirstName = accountUser.FirstName,
                LastName = accountUser.LastName,
                HashedPassword = accountUser.HashedPassword,
                Email = accountUser.Email,
                DateOfBirth = accountUser.DateOfBirth,
                Address = accountUser.Address,
                ZIP = accountUser.ZIP,
                MobilePhone = accountUser.MobilePhone,
                City = accountUser.City,
                IBAN = accountUser.IBAN,
                CreationDate = accountUser.CreationDate,
                IsAdmin = accountUser.IsAdmin,
                IsActive = accountUser.IsActive,
                IsQienEmployee = accountUser.IsQienEmployee,
                IsSeniorDeveloper = accountUser.IsSeniorDeveloper,
                IsTrainee = accountUser.IsTrainee,
                ImageProfileString = accountUser.ProfileImage
            };
            
            return View(tempacc);
        }

        //Updaten personalia na het wijzigen en opslaan
        [HttpPost]
        public async Task<IActionResult> EmployeePersonalia(EmployeeUpdateAccountModel updatedAccount)
        {

            if (!ModelState.IsValid)
            {
                return View(updatedAccount);
            }
            string uniqueFilename = updatedAccount.ImageProfileString;
            var existingAccount = await accountRepo.GetOneAccount(updatedAccount.AccountId);
            if (updatedAccount.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images/ProfileImages");
                string filePath = Path.Combine(uploadsFolder, updatedAccount.ImageProfileString);
                uniqueFilename = updatedAccount.ImageProfileString;
                System.IO.File.Delete(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    updatedAccount.ProfileImage.CopyTo(stream);
                }
            }
            if (existingAccount == null)
            {
                return View(updatedAccount);
            }
            AccountModel acc = new AccountModel()
            {
                AccountId = updatedAccount.AccountId,
                FirstName = updatedAccount.FirstName,
                LastName = updatedAccount.LastName,
                Email = updatedAccount.Email,
                DateOfBirth = updatedAccount.DateOfBirth,
                Address = updatedAccount.Address,
                ZIP = updatedAccount.ZIP,
                MobilePhone = updatedAccount.MobilePhone,
                City = updatedAccount.City,
                IBAN = updatedAccount.IBAN,
                CreationDate = updatedAccount.CreationDate,
                ProfileImage = uniqueFilename,
                IsAdmin = updatedAccount.IsAdmin,
                IsActive = updatedAccount.IsActive,
                IsQienEmployee = updatedAccount.IsQienEmployee,
                IsSeniorDeveloper = updatedAccount.IsSeniorDeveloper,
                IsTrainee = updatedAccount.IsTrainee,
                IsChanged = updatedAccount.IsChanged

            };

            acc.IsChanged = true;

            await accountRepo.UpdateAccount(acc, uniqueFilename);
            ViewBag.imageurl = uniqueFilename;
            return RedirectToRoute(new { controller = "Employee", action = "EmployeeDashboard", accountId = acc.AccountId });
        }

        //verwijzing naar ChangePassword pagina
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //Veranderen van wachtwoord voor de huidige gebruiker
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                var user = await _userManager.GetUserAsync(User);
                if(user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if(!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            
        }

        //Ophalen van alle urenformulieren per jaar voor de gebruiker die is ingelogd
        [HttpGet]
        public async Task<IActionResult> YearOverview(int year = 0)
        {

            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var formListYears = await hoursformRepo.GetAllYearsForUser(id);
            List<SelectListItem> SelectListYears = new List<SelectListItem>();

            var selected = false;
            foreach(int y in formListYears)
            {
                if (y == year)
                {
                    selected = true;
                }

                SelectListYears.Add(
                    new SelectListItem
                    {
                        Text = y.ToString(),
                        Value = y.ToString(),
                        Selected = selected
                    }
                );

                selected = false;
            }

            ViewBag.formYears = SelectListYears;


            List<string> months = new List<string>() { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
           
            List<HoursFormModel> forms = await hoursformRepo.GetAllFormsForAccountForYear(year, id);
            List<HoursFormModel> sorted = new List<HoursFormModel>();

            
                foreach(var month in months)
                {
                    foreach (var form in forms)
                    {
                        if (form.ProjectMonth == month)
                        {
                            sorted.Add(form);
                        }

                    }
                }

            ViewBag.accountId = id;


            return View(sorted);
        }

    }
}
