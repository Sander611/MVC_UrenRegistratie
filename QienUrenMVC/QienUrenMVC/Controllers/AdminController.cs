using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using MailKit.Net.Smtp;

namespace QienUrenMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IAccountRepository accountRepo;
        private readonly IClientRepository clientRepo;
        private readonly IHoursFormRepository hoursformRepo;
        private readonly IHoursPerDayRepository hoursperdayRepo;

        public AdminController(
                                IWebHostEnvironment hostingEnvironment,
                                IAccountRepository AccountRepo,
                                IClientRepository ClientRepo,
                                IHoursFormRepository HoursFormRepo,
                                IHoursPerDayRepository HoursPerDayRepo)
        {
            this.hostingEnvironment = hostingEnvironment;
            accountRepo = AccountRepo;
            clientRepo = ClientRepo;
            hoursformRepo = HoursFormRepo;
            hoursperdayRepo = HoursPerDayRepo;
        }

        // method om als admin op een gebruiker te klikken en alle forms als overzicht te krijgen (doormiddel van dropdownmenus voor maand, jaar )

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {

            List<AdminTaskModel> uncheckedForms = await hoursformRepo.GetAllClientAcceptedForms();


            return View(uncheckedForms);
        }

        [HttpGet]
        public async Task<IActionResult> Controleren(int formId, string accountId, string fullName, string month, string year)
        {

            ViewBag.formId = formId;
            ViewBag.accountId = accountId;
            ViewBag.fullName = fullName;
            ViewBag.month = month;
            ViewBag.year = year;

            List<HoursPerDayModel> formsForId = await hoursperdayRepo.GetAllDaysForForm(formId);

            return View(formsForId);
        }

        [HttpGet]
        public async Task<IActionResult> AccountOverzicht(string searchString)
        {

            List<AccountModel> listAccounts = await accountRepo.GetAllAccounts(searchString);

            return View(listAccounts);
        }

        [HttpGet]
        public async Task<IActionResult> UrenFormulieren(string id, string name)
        {
            ViewBag.currUser = name;

            List<HoursFormModel> allFormsForAccount = await hoursformRepo.GetSingleAccountForms(id);

            return View(allFormsForAccount);
        }


        public async Task<IActionResult> DeleteAccount(string accountID)
        {

            string succesfull = accountRepo.RemoveAccount(accountID);

            return RedirectToRoute(new { controller = "Admin", action = "AccountOverzicht" });
        }

        [HttpGet]
        public async Task<IActionResult> EditAccount (string accountID)
        {
            AccountModel userInfo = await accountRepo.GetOneAccount(accountID);

            ViewBag.currUser = userInfo.FirstName +" "+ userInfo.LastName;

            return View(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(AccountModel updatedAccount)
        {

            if (ModelState.IsValid)
            {
                var existingAccount = await accountRepo.GetOneAccount(updatedAccount.AccountId);
                if(existingAccount == null)
                {
                    return View(updatedAccount);
                }
                AccountModel acc = await accountRepo.UpdateAccount(updatedAccount);

                return RedirectToRoute(new { controller = "Admin", action = "AccountOverzicht" });
            }

            return View(updatedAccount);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            return View();
        }        
        
        
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(AccountModelCreateView model)
        {
            if(ModelState.IsValid)
            {

                //var formFile = HttpContext.Request.Form.Files[0];
                string uniqueFilename = "user-circle-solid.svg";
                if(model.ProfileImage != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images/ProfileImages");
                    uniqueFilename = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFilename);
                    model.ProfileImage.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                AccountModel newAccount = new AccountModel()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    HashedPassword = model.Password,
                    Address = model.Address,
                    ZIP = model.ZIP,
                    MobilePhone = model.MobilePhone,
                    City = model.City,
                    IBAN = model.IBAN,
                    CreationDate = model.CreationDate,
                    ProfileImage = uniqueFilename,
                    IsAdmin = model.IsAdmin,
                    IsActive = model.IsActive,
                    IsQienEmployee = model.IsQienEmployee,
                    IsSeniorDeveloper = model.IsSeniorDeveloper,
                    IsTrainee = model.IsTrainee
                };
                AccountModel acc = await accountRepo.AddNewAccount(newAccount);

                DateTime day = DateTime.Today;
                DateTime? date = DateTime.Now;
                var firstDay = new DateTime(day.Year, day.Month, 1);
                int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                var ProjectMonth = date.Value.ToString("MMMM").ToLower();

                switch (ProjectMonth)
                {
                    case "January":
                        ProjectMonth = "januari";
                        break;
                    case "February":
                        ProjectMonth = "februari";
                        break;
                    case "March":
                        ProjectMonth = "maart";
                        break;
                    case "April":
                        ProjectMonth = "april";
                        break;
                    case "May":
                        ProjectMonth = "mei";
                        break;
                    case "Juny":
                        ProjectMonth = "juni";
                        break;
                    case "July":
                        ProjectMonth = "juli";
                        break;
                    case "Augustus":
                        ProjectMonth = "augustus";
                        break;
                    case "September":
                        ProjectMonth = "september";
                        break;
                    case "Oktober":
                        ProjectMonth = "oktober";
                        break;
                    case "November":
                        ProjectMonth = "november";
                        break;
                    case "December":
                        ProjectMonth = "december";
                        break;
                }

                HoursFormModel hoursForm = new HoursFormModel()
                {
                    AccountId = acc.AccountId,
                    ProjectMonth = ProjectMonth,
                    Year = DateTime.Now.Year,
                    DateDue = firstDay.AddDays(days + 5)
                };

                await hoursformRepo.CreateNewForm(hoursForm);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("QienUrenRegistratie", "GroepTweeQien@gmail.com"));
                message.To.Add(new MailboxAddress($"{acc.FirstName} {acc.LastName}", acc.Email));
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

        
        [HttpGet]
        public async Task<IActionResult> YearOverview(int Year)
        {
            List<string> allTraineesIds = await accountRepo.GetAccountIdsByRole("Trainee");
            List<string> allEmployeesIds = await accountRepo.GetAccountIdsByRole("Employee");
            List<string> allSoftwareDevelopersIds = await accountRepo.GetAccountIdsByRole("SoftwareDeveloper");

            List<YearOverviewModel> OverviewList = await hoursformRepo.GetYearOverviews(Year, allTraineesIds, allEmployeesIds, allSoftwareDevelopersIds);

            return View(OverviewList);
        }

        [HttpGet]
        public async Task<IActionResult> FormsForYear(int year, string month)
        {
          
            List<HoursFormModel> specificFormsForDate = await hoursformRepo.GetFormsForYearAndMonth(year, month);
            ViewBag.Year = year;
            ViewBag.Month = month;
            return View(specificFormsForDate);
        }

        [HttpGet]
        public async Task<IActionResult> CheckControleren()
        {
            return View();
        }



    }
}