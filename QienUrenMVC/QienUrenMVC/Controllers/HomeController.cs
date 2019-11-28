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


namespace QienUrenMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;

        private Task<UserIdentity> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<string> GetCurrentUserId()
        {
            UserIdentity user = await GetCurrentUserAsync();
            return user?.Id;
        }

        public IActionResult Index()
        {

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;


            if (currentUser.IsInRole("Admin") == true)
            {
                return RedirectToRoute(new { controller = "Admin", action = "Dashboard" });
            }
            if (User.IsInRole("Employee") == true)
            {
                return Redirect("Employee/Dashboard");
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
