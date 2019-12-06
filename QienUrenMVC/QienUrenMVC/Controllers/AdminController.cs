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
using Microsoft.AspNetCore.Identity;
using QienUrenMVC.Data;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        private readonly UserManager<UserIdentity> _userManager;

        public AdminController(
                                IWebHostEnvironment hostingEnvironment,
                                IAccountRepository AccountRepo,
                                IClientRepository ClientRepo,
                                IHoursFormRepository HoursFormRepo,
                                IHoursPerDayRepository HoursPerDayRepo,
                                UserManager<UserIdentity> userManager)
        {
            this.hostingEnvironment = hostingEnvironment;
            accountRepo = AccountRepo;
            clientRepo = ClientRepo;
            hoursformRepo = HoursFormRepo;
            hoursperdayRepo = HoursPerDayRepo;
            _userManager = userManager;
        }

        // method om als admin op een gebruiker te klikken en alle forms als overzicht te krijgen (doormiddel van dropdownmenus voor maand, jaar )

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            AdminTaskModel adminTaskModel = new AdminTaskModel
            {
                uncheckedForms = await hoursformRepo.GetAllClientAcceptedForms(),
                changedAccounts = await accountRepo.GetChangedAccounts(),

            };
            


            //List<AdminTaskModel> uncheckedForms = await hoursformRepo.GetAllClientAcceptedForms();
            //List<AccountModel> changedaccounts = await accountRepo.GetChangedAccounts();

            return View(adminTaskModel);
        }

        [HttpGet]
        public async Task<IActionResult> Controleren(int formId, string accountId, string fullName, string month, string year, int state)
        {

            ViewBag.formId = formId;
            ViewBag.accountId = accountId;
            ViewBag.fullName = fullName;
            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.status = state;


            HoursFormModel formInfo = await hoursformRepo.GetFormById(formId);
            ViewBag.textAdmin = formInfo.CommentAdmin;
            ViewBag.textClient = formInfo.CommentClient;
            ViewBag.TotalHours = formInfo.TotalHours;
            ViewBag.TotalSick = formInfo.TotalSick;
            ViewBag.TotalOver = formInfo.TotalOver;
            ViewBag.TotalLeave = formInfo.TotalLeave;
            ViewBag.TotalTraining = formInfo.TotalTraining;
            ViewBag.TotalOther = formInfo.TotalOther;

            List <HoursPerDayModel> formsForId = await hoursperdayRepo.GetAllDaysForForm(formId);

            return View(formsForId);
        }

        [HttpGet]
        public async Task<IActionResult> PersonaliaControleren(string accountId)
        {
            UserPersonaliaModel personaliaModel = await accountRepo.ComparePersonaliaChanges(accountId);
            //HoursFormModel formInfo = await hoursformRepo.GetFormById(formId);
            //ViewBag.textAdmin = formInfo.CommentAdmin;
            //ViewBag.textClient = formInfo.CommentClient;


            //List<HoursPerDayModel> formsForId = await hoursperdayRepo.GetAllDaysForForm(formId);

            return View(personaliaModel);
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

            await accountRepo.RemoveAccount(accountID);
            await accountRepo.RemoveAllFormPerAccount(accountID);

            return RedirectToRoute(new { controller = "Admin", action = "AccountOverzicht" });
        }

        [HttpGet]
        public async Task<IActionResult> EditAccount(string accountID)
        {
            AccountModel accountUser = await accountRepo.GetOneAccount(accountID);
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

            ViewBag.currUser = accountUser.FirstName + " " + accountUser.LastName;

            return View(tempacc);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(EmployeeUpdateAccountModel updatedAccount)
        {

            if (ModelState.IsValid)
            {
                var uniqueFilename = "";
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
                    updatedAccount.ProfileImage.CopyTo(new FileStream(filePath, FileMode.Create));
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
                    IsTrainee = updatedAccount.IsTrainee
                };
                await accountRepo.UpdateAccount(acc, uniqueFilename);
                return RedirectToRoute(new { controller = "Admin", action = "AccountOverzicht", accountId = acc.AccountId });
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
            if (ModelState.IsValid)
            {

                //var formFile = HttpContext.Request.Form.Files[0];
                string uniqueFilename = "user-circle-solid.svg";
                if (model.ProfileImage != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images/ProfileImages");
                    uniqueFilename = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFilename);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ProfileImage.CopyTo(stream);
                    }
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


                var user = await _userManager.FindByEmailAsync(acc.Email);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var email = acc.Email;
                //var link = $"https://localhost:44306/Identity/Account/ResetPassword?email={acc.Email}&token={encodedToken}";

                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", email, token },
                    protocol: Request.Scheme);

                string tempPassword = acc.HashedPassword;
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("QienUrenRegistratie", "GroepTweeQien@gmail.com"));
                message.To.Add(new MailboxAddress($"{acc.FirstName} {acc.LastName}", acc.Email));
                message.Subject = "New account was created";

                message.Body = new TextPart("plain")
                {
                    Text = $"An account with this email was created. Here you can reset you password : {callbackUrl}"
                };
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("Smtp.gmail.com", 587, false);
                    client.Authenticate("GroepTweeQien@gmail.com", "GroepQien2019!");
                    client.Send(message);
                    client.Disconnect(true);
                }
                return RedirectToRoute(new { controller = "Admin", action = "AccountOverzicht" });
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> YearOverview(int Year = 0)
        {
            if (Year == 0)
            {
                Year = DateTime.Now.Year;
            }

            var formListYears = await hoursformRepo.GetAllExistingYears(Year);
            List<SelectListItem> SelectListYears = new List<SelectListItem>();

            var selected = false;
            foreach (int y in formListYears)
            {
                if (y == Year)
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

            List<string> allTraineesIds = await accountRepo.GetAccountIdsByRole("Trainee");
            List<string> allEmployeesIds = await accountRepo.GetAccountIdsByRole("Employee");
            List<string> allSoftwareDevelopersIds = await accountRepo.GetAccountIdsByRole("SoftwareDeveloper");

            List<YearOverviewModel> OverviewList = await hoursformRepo.GetYearOverviews(Year, allTraineesIds, allEmployeesIds, allSoftwareDevelopersIds);

            return View(OverviewList);
        }

        [HttpGet]
        public async Task<IActionResult> FormsForYear(int year, string month)
        {

            List<FormsForMonthModel> specificFormsForDate = await hoursformRepo.GetFormsForYearAndMonth(year, month);
            
            ViewBag.Year = year;
            ViewBag.Month = month;

            int totalHours = 0;
            int totalSick = 0;
            int totalOver = 0;
            int totalLeave = 0;
            int totalTraining = 0;
            int totalOther = 0;
            foreach (var form in specificFormsForDate)
            {
                totalHours += form.TotalHours;
                totalSick += form.TotalSick;
                totalOver += form.TotalOver;
                totalLeave += form.TotalLeave;
                totalTraining += form.TotalTraining;
                totalOther += form.TotalOther;
            }

            ViewBag.TotalHours = totalHours;
            ViewBag.TotalSick = totalSick;
            ViewBag.TotalOver = totalOver;
            ViewBag.TotalLeave = totalLeave;
            ViewBag.TotalTraining = totalTraining;
            ViewBag.TotalOther = totalOther;
            return View(specificFormsForDate);
        }

        [HttpGet("{keuring}/{id}")]
        public async Task<IActionResult> CheckControleren(string keuring, int id, string adminText, string clientText, string CCaccountId, string CCfullName, string CCmonth, string CCyear, int CCstate)
        {

            if (keuring == "true")
            {
                await hoursformRepo.ChangeState(3, id, clientText, adminText);
                CCstate = 3;
            }
            else if (keuring == "false")
            {
                await hoursformRepo.ChangeState(4, id, clientText, adminText);
                CCstate = 4;
            }

            return RedirectToRoute(new { controller = "Admin", action = "Controleren", formId = id, accountId = CCaccountId, fullName = CCfullName, month = CCmonth, year = CCyear, state = CCstate });

        }



    }
}