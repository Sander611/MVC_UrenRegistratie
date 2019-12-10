using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using QienUrenMVC.Data;
using Microsoft.AspNetCore.Identity;
using static Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal.ExternalLoginModel;
using Microsoft.Extensions.Logging;

namespace QienUrenMVC.Controllers
{

    public class PasswordController : Controller
    {

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IAccountRepository accountRepo;
        private readonly IClientRepository clientRepo;
        private readonly IHoursFormRepository hoursformRepo;
        private readonly IHoursPerDayRepository hoursperdayRepo;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly ILogger<PasswordController> _logger;
        public PasswordController(
                            IWebHostEnvironment hostingEnvironment,
                            IAccountRepository AccountRepo,
                            IClientRepository ClientRepo,
                            IHoursFormRepository HoursFormRepo,
                            IHoursPerDayRepository HoursPerDayRepo,
                            ILogger<PasswordController> logger)
        {
            this.hostingEnvironment = hostingEnvironment;
            accountRepo = AccountRepo;
            clientRepo = ClientRepo;
            hoursformRepo = HoursFormRepo;
            hoursperdayRepo = HoursPerDayRepo;
            _logger = logger;
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(InputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

                    _logger.Log(LogLevel.Warning, passwordResetLink);

                    return View("ForgotPasswordConfirmation");

                }
                return View("ForgotPasswordConfirmation");
        }
    }
}