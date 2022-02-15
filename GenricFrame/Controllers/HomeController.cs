using GenricFrame.AppCode.CustomAttributes;
using GenricFrame.AppCode.Extensions;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Migrations;
using GenricFrame.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace GenricFrame.Controllers
{
      [Authorize]
    //[JWTAuthorize]
    public class HomeController : Controller
    {
        private IUserService _userService;
        private IHttpContextAccessor _httpContext;
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider IServiceProvider;
        private readonly AppicationUser _user;
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContext, IUserService userService, IServiceProvider ServiceProvider)
        {
            _userService = userService;
            _httpContext = httpContext;
            _logger = logger;
            IServiceProvider = ServiceProvider;
            _user = (Models.AppicationUser)_httpContext.HttpContext.Items["User"];
        }

        public IActionResult Index()
        {
            var userId = User.GetLoggedInUserId<string>(); // Specify the type of your UserId;
            var userName = User.GetLoggedInUserName();
            var userEmail = User.GetLoggedInUserEmail();
            return View();
        }

        [ValidateAjax]
        public IActionResult DemoModalValidation(DemoViewModel demo)
        {
            return View();
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

        [HttpGet]
        [JWTAuthorize]
        public ActionResult Get()
        {
            return Json(_user);
        }

        [HttpPost, Route(nameof(RunMigration))]
        public IActionResult RunMigration(string DatabaseName)
        {
            var result = MigrationManager.MigrateDatabase(IServiceProvider, DatabaseName);
            return Json(result);
        }
    }
}
