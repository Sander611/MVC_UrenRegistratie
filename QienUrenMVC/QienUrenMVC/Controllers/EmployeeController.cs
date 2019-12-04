﻿using System;
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
        //private readonly ApiHelper helper;
        //public EmployeeController(ApiHelper helper)
        //{
        //    this.helper = helper;
        //}
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

            AccountModel result = await accountRepo.GetOneAccount(accountId);

            List<HoursFormModel> result1 = await hoursformRepo.getAllFormPerAccount(accountId);


            AccountModel accountInfo = new AccountModel()
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Address = result.Address,
                ZIP = result.ZIP,
                AccountId = result.AccountId,
                City = result.City,
                ProfileImage = result.ProfileImage
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
                    ProjectMonth = form.ProjectMonth,
                    IsAcceptedClient = form.IsAcceptedClient
                });
            }
            
            EmployeeDashboardModel EDM = new EmployeeDashboardModel();
            EDM.Account = accountInfo;
            EDM.Forms = formsOverview;
            return View(EDM);
        }

        [HttpGet]
        public async Task<IActionResult> HoursRegistration(int formid, int state)
        {
            List<HoursPerDayModel> formsForId = await hoursperdayRepo.GetAllDaysForForm(formid);
            var clientList = hoursperdayRepo.GetClientList();
            ViewBag.CompanyNames = clientList;

            ViewBag.FormId = formid;
            ViewBag.month = formsForId[0].Month;
            ViewBag.year = await hoursformRepo.GetYearOfForm(formid);
            ViewBag.status = state;
            if (state == 4)
            {
                HoursFormModel formInfo = await hoursformRepo.GetFormById(formid);
                ViewBag.textAdmin = formInfo.CommentAdmin;
                ViewBag.textClient = formInfo.CommentClient;
            }
            return View(formsForId);
        }

        [HttpPost]
        public async Task<IActionResult> HoursRegistration(List<HoursPerDayModel> model, bool versturen, int formid)
        {
            AccountModel medewerkerInfo = await accountRepo.GetAccountByFormId(formid);
            string Name = $"{medewerkerInfo.FirstName}{medewerkerInfo.LastName}";
            HoursFormModel hoursForm = await hoursformRepo.GetFormsById(formid);
            var clientList = hoursperdayRepo.GetClientList();
            ViewBag.CompanyNames = clientList;
            ViewBag.month = model[0].Month;
            ViewBag.year = await hoursformRepo.GetYearOfForm(model[0].FormId);

            if (ModelState.IsValid)
            {
                List<HoursPerDayModel> hpdModel = await hoursperdayRepo.Update(model);

                int totalHours = 0;
                foreach(var perday in model)
                {
                    totalHours += perday.Hours;
                }

                await hoursformRepo.UpdateTotalHoursForm(model[0].FormId, totalHours);
                ViewBag.status = 0;

                if (versturen == true)
                {

                    var clientIds = model.Select(m => m.ClientId).Distinct();
                    foreach (var client in clientIds)
                    {

                        var verificationCode = Guid.NewGuid();
                        var varifyUrl = $"https://localhost:44306/Client/ControlerenClient?formId={formid}&accountId={medewerkerInfo.AccountId}&fullName={Name}&month={hoursForm.ProjectMonth}&year={hoursForm.Year}";
                        ClientModel client1 = await clientRepo.GetById(client.GetValueOrDefault());
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress("QienUrenRegistratie", "GroepTweeQien@gmail.com"));
                        message.To.Add(new MailboxAddress($"{client1.ClientName1}", client1.ClientEmail1));
                        message.Subject = "Check formulier";
                        message.Body = new TextPart("plain")
                        {
                            Text = $"We would like you to check hours registration declaration it was filled by {Name}. Please click on the below link to check the data :<a href='{varifyUrl}'>link</a>"
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
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFormForAccount(HoursFormModel hoursformModel)
        {
            hoursformModel.AccountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            hoursformModel.DateSend = DateTime.Now;
            hoursformModel.TotalHours = 563;
            hoursformModel.ProjectMonth = "september";
            hoursformModel.Year = 2019;
            hoursformModel.IsAcceptedClient = 4;
            hoursformModel.IsLocked = false;
            hoursformModel.CommentAdmin = "";
            hoursformModel.CommentClient = "";

            var result = await hoursformRepo.CreateNewForm(hoursformModel);

            return RedirectToRoute(new { controller = "Employee", action = "EmployeeDashboard" });
        }

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

        [HttpPost]
        public async Task<IActionResult> EmployeePersonalia(EmployeeUpdateAccountModel updatedAccount)
        {

            if (ModelState.IsValid)
            {
                string uniqueFilename = "";
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
                return RedirectToRoute(new { controller = "Employee", action = "EmployeeDashboard", accountId = acc.AccountId});
            }

            return View(updatedAccount);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
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

            return View(model);
        }

    }
}
