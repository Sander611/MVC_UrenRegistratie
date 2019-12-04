using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QienUrenMVC.Data;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;

namespace QienUrenMVC.Controllers
{
    public class HomeController : Controller
    {
        private List<string> months = new List<string>() { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };

        private readonly ILogger<HomeController> _logger;
        private readonly IHoursFormRepository hoursformRepo;

        public HomeController(IHoursFormRepository HoursFormRepo,  ILogger<HomeController> logger)
        {
            _logger = logger;
            hoursformRepo = HoursFormRepo;
        }

        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;

        private Task<UserIdentity> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<string> GetCurrentUserId()
        {
            UserIdentity user = await GetCurrentUserAsync();
            return user?.Id;
        }

        public async Task<IActionResult> Index(string accountId)
        {

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;


            if (currentUser.IsInRole("Admin") == true)
            {
                return RedirectToRoute(new { controller = "Admin", action = "Dashboard" });
            }
            if (User.IsInRole("Employee") == true)
            {
                DateTime day = DateTime.Today;
                var firstDay = new DateTime(day.Year, day.Month, 1);
                int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                HoursFormModel form = await hoursformRepo.CheckIfExists(User.FindFirstValue(ClaimTypes.NameIdentifier), months[day.Month - 1], day.Year);

                if (form == null)
                {


                    HoursFormModel hoursformModel = new HoursFormModel()
                    {

                        AccountId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        IsAcceptedClient = 0,
                        ProjectMonth = months[day.Month - 1],
                        Year = day.Year,
                        IsLocked = false,
                        CommentAdmin = "",
                        CommentClient = "",
                        TotalHours = 0
                    };
                    var result = await hoursformRepo.CreateNewForm(hoursformModel);
                }


                return RedirectToRoute(new { controller = "Employee", action = "EmployeeDashboard", accountId = User.FindFirstValue(ClaimTypes.NameIdentifier) });
            }
            else
            {
                return Redirect("Identity/Account/Login");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

}
