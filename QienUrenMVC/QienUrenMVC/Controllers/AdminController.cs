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
using Microsoft.AspNetCore.Http;

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

        public List<string> months = new List<string>() { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };

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


        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            ///
            /// This method loads the view of the Admin dashboard
            /// The view will get the following data:
            /// "uncheckedForms", containing all hourforms that are checked by the Client.
            /// "changedAccounts", containing all requests from users that have changed their personal data.
            /// "allHoursYear", containing all total hours from every user and displays it together as total for different sections.
            ///

            DateTime dt = DateTime.Now;
            var currYear = dt.Year;
            AdminTaskModel adminTaskModel = new AdminTaskModel
            {
                uncheckedForms = await hoursformRepo.GetAllClientAcceptedForms(),
                changedAccounts = await accountRepo.GetChangedAccounts(),
                allHoursYear = await hoursformRepo.GetAllHoursYear(currYear)

            };

            var roles = new List<SelectListItem>
            {
                new SelectListItem{Value = "1", Text = "Iedereen"},
                new SelectListItem{Value = "2", Text = "Trainees"},
                new SelectListItem{Value = "3", Text = "Qien Medewerker"},
                new SelectListItem{Value = "4", Text = "Senior Medewerker"}
            };
            ViewBag.Roles = roles;

            return View(adminTaskModel);
        }

        [HttpPost]
        public async Task<IActionResult> ApprovePersonalia(string accountId)
        {
            ///
            /// This method is executed when the Admin accepts changes made by a User for his/hers personalia.
            /// A method in the AccountRepository gets called in which the changes will be implemented.
            ///

            await accountRepo.SetAccountChanged(accountId, false);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DisapprovePersonalia(string accountId)
        {
            ///
            /// This method is executed when the Admin declines changes made by a User for his/hers personalia.
            /// A method in the AccountRepository gets called in which the request will be dismissed.
            ///

            await accountRepo.RevertAccountPersonalia(accountId);
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Controleren(int formId, string accountId, string fullName, string month, string year, int state)
        {
            ///
            /// This method prepares the "controleren" view in which the admin is able to see the users hourforms and 
            /// accept or decline send hourforms.
            /// It makes a call to the hoursperdayRepository to get the hours filled in by an user for each day.
            ///

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
            ///
            /// This method generates the view in which an admin can accept or dismiss the users changes made to their own personalia.
            ///

            UserPersonaliaModel personaliaModel = await accountRepo.ComparePersonaliaChanges(accountId);

            return View(personaliaModel);
        }

        [HttpGet]
        public async Task<IActionResult> AccountOverzicht(string searchString)
        {
            ///
            /// This method gets all necessary data for the view "AccountOverzicht", in which the Admin can view all accounts existing
            /// in the application.
            /// In order to get this list of accounts a call is made to the AccountRepository.
            ///

            List<AccountModel> listAccounts = await accountRepo.GetAllAccounts(searchString);

            return View(listAccounts);
        }

        [HttpGet]
        public async Task<IActionResult> UrenFormulieren(string id, string name, int year)
        {
            ///
            /// This method gets all hourforms for a specific user. It also generates data for an dropdown element to select years.
            /// It uses the HourFormRepository to obtain this data. 
            /// At the end of the method the data gets sorted on month (jan -> dec).
            ///
            ViewBag.currUser = name;
            ViewBag.currId = id;

            var formListYears = await hoursformRepo.GetAllYearsForUser(id);
            List<SelectListItem> SelectListYears = new List<SelectListItem>();

            var selected = false;
            foreach (int y in formListYears)
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

            List<HoursFormModel> allFormsForAccount = await hoursformRepo.GetSingleAccountForms(id, year);
            List<HoursFormModel> sorted = new List<HoursFormModel>();

            foreach (var month in months)
            {
                foreach (var form in allFormsForAccount)
                {
                    if (form.ProjectMonth == month)
                    {
                        sorted.Add(form);
                    }

                }
            }

            return View(sorted);
        }


        public async Task<IActionResult> DeleteAccount(string accountID)
        {
            ///
            /// This method calls methods in the AccountRepository to delete an User and its hourforms/hourperdays data.
            ///

            await accountRepo.RemoveAccount(accountID);
            await accountRepo.RemoveAllFormPerAccount(accountID);

            return RedirectToRoute(new { controller = "Admin", action = "AccountOverzicht" });
        }

        [HttpGet]
        public async Task<IActionResult> EditAccount(string accountID)
        {
            ///
            /// This method gets all data from a specific user using the id. And creates a view where the Admin can edit the users data.
            ///
           
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
            ///
            /// After editing an User the model is checked here for validation. If validation is ok the user gets updated by
            /// sending the data as model to the AccountRepository.
            ///

            if (!ModelState.IsValid)
            {
                return View(updatedAccount);
            }
            else
            {
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
                await accountRepo.AdminUpdateAccount(acc, uniqueFilename);
                return RedirectToRoute(new { controller = "Admin", action = "AccountOverzicht", accountId = acc.AccountId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            ///
            /// This method generates the View in which the Admin can create a new User.
            ///

            var clientList = hoursperdayRepo.GetClientList();
            ViewBag.CompanyNames = clientList;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmployee(AccountModelCreateView model, int ClientId)
        {
            ///
            /// This method gets an Model containing data for a new User account.
            /// If an photo is uploaded the user obtains an unique string in which his/hers photo gets stored.
            /// Also a hoursform gets generated for the specific month in which the Admin is creating the account.
            /// At the end an email gets created that is send to the User. In this email an User obtains a link to reset its password.
            ///

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else 
            {
                
                string uniqueFilename = "profilephoto.png";
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
                if (model.ProfileImage == null)
                {
                    string filename = "profilephoto.png";
                    string sourcePath = Path.Combine(hostingEnvironment.WebRootPath, "Images/ProfileImages");
                    string targetPath = Path.Combine(hostingEnvironment.WebRootPath, "Images/ProfileImages");
                    string sourceFile = Path.Combine(sourcePath, filename);
                    uniqueFilename = Guid.NewGuid().ToString() + "_" + model.AccountId + filename;
                    string destFile = Path.Combine(targetPath, uniqueFilename);

                    System.IO.File.Copy(sourceFile, destFile, true);
                }

                var clientList = hoursperdayRepo.GetClientList();
                ViewBag.CompanyNames = clientList;

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
                    CreationDate = DateTime.Now,
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

                await hoursformRepo.CreateNewForm(hoursForm, ClientId);


                var user = await _userManager.FindByEmailAsync(acc.Email);
                var passtoken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passtoken));
                var email = acc.Email;

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
                    Text = $"An account with this email was created. Here you can reset you password :{callbackUrl}"
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
        }


        [HttpGet]
        public async Task<IActionResult> YearOverview(int Year)
        {
            ///
            /// This method gets all the account Ids for different roles (trainee, employee, sr. software developer) 
            /// All theses ids are then used to calculate all total hours of each role and is then displayed in a View.
            ///

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
            ///
            /// This method gets all the Hoursforms for an specific year (all users).
            /// It also calculates the total of each column (for that year).
            ///

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
            ///
            /// If the Admin accepts or declines a hoursform send in by an User the script will come here and update data in the database.
            /// State 3 == Accepted by Admin.
            /// State 4 == Declined by Admin.
            ///

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