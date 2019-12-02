using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IWebHostEnvironment hostingEnvironment;


        public EmployeeController(
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
        public async Task<IActionResult> HoursRegistration(List<HoursPerDayModel> model)
        {
            var clientList = hoursperdayRepo.GetClientList();
            ViewBag.CompanyNames = clientList;

            if (ModelState.IsValid)
            {
                List<HoursPerDayModel> hpdModel = await hoursperdayRepo.Update(model);
                return View(model);
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
            ViewBag.imageurl = accountUser.ProfileImage;
            return View(accountUser);
        }

        [HttpPost]
        public async Task<IActionResult> EmployeePersonalia(EmployeeUpdateAccountModel updatedAccount)
        {

            if (ModelState.IsValid)
            {
                var existingAccount = await accountRepo.GetOneAccount(updatedAccount.AccountId);
                string uniqueFilename = "";
                if (updatedAccount.ProfileImage != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images/ProfileImages");
                    uniqueFilename = Guid.NewGuid().ToString() + "_" + updatedAccount.ProfileImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFilename);
                    updatedAccount.ProfileImage.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                if (existingAccount == null)
                {
                    return View(updatedAccount);
                }
                AccountModel acc = new AccountModel()
                {
                    FirstName = updatedAccount.FirstName,
                    LastName = updatedAccount.LastName,
                    Email = updatedAccount.Email,
                    DateOfBirth = updatedAccount.DateOfBirth,
                    HashedPassword = updatedAccount.Password,
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
                

                return RedirectToRoute(new { controller = "Employee", action = "EmployeeDashboard" });
            }

            return View(updatedAccount);
        }

    }
}
