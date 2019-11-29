﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> EmployeeDashboard()
        {

            AccountModel result = await accountRepo.GetOneAccount("dfe8f40b-558f-4ca2-8af1-72a703c44df3");

            List<HoursFormModel> result1 = await hoursformRepo.getAllFormPerAccount("dfe8f40b-558f-4ca2-8af1-72a703c44df3");


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
            List<HoursFormModel> hoursforms = new List<HoursFormModel>();

            //HttpClient client = _api.Connect();
            //HttpResponseMessage res = await client.GetAsync("HoursForm/hoursform");

            //if(res.IsSuccessStatusCode)
            //{
            //    var result = res.Content.ReadAsStringAsync().Result;
            //    hoursforms = JsonConvert.DeserializeObject<List<HoursFormModel>>(result);
            //}

            for (int i = 0; i < 5; i++)
            {
                HoursFormModel hoursform = new HoursFormModel()
                {
                    ProjectMonth = "November",
                    DateDue = new DateTime(2019,02,12)

                };

                hoursforms.Add(hoursform);
            }
            return View(EDM);
        }

        [HttpGet]
        public async Task<IActionResult> HoursRegistration(int formid)
        {
            List<HoursPerDayModel> formsForId = await hoursperdayRepo.GetAllDaysForForm(formid);


            return View(formsForId);
        }

        [HttpPost]
        public async Task<IActionResult> HoursRegistration(List<HoursPerDayModel> model)
        {
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

            hoursformModel.AccountId = "dfe8f40b-558f-4ca2-8af1-72a703c44df3";
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
    }
}
