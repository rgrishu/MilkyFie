using AutoMapper;
using GenricFrame.AppCode.CustomAttributes;
using GenricFrame.AppCode.Extensions;
using GenricFrame.AppCode.Helper;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Migrations;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
        private readonly IRepository<EmailConfig> _emailConfig;
        private IMapper _mapper;
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContext, IUserService userService, IServiceProvider ServiceProvider, IRepository<EmailConfig> emailConfig, IMapper mapper)
        {
            _userService = userService;
            _httpContext = httpContext;
            _logger = logger;
            _emailConfig = emailConfig;
            IServiceProvider = ServiceProvider;
            _mapper = mapper;
            if (_httpContext!=null && _httpContext.HttpContext != null)
            {
                _user = (AppicationUser)_httpContext?.HttpContext.Items["User"];
            }
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

        public void NLog()
        {
            _logger.LogInformation("Requested a Random API");
            int count;
            try
            {
                for (count = 0; count <= 5; count++)
                {
                    if (count == 3)
                    {
                        throw new Exception("Random Exception Occured");
                    }
                    else
                    {
                        _logger.LogInformation("Iteration Count is {iteration}", count);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
            }
        }

        [HttpPost]
        [Route("welcome")]
        public IActionResult Welcome(string userName)
        {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeMail(userName));
            return Ok($"Job Id {jobId} Completed. Welcome Mail Sent!");
        }

        public void SendWelcomeMail(string userName)
        {
            var config = _emailConfig.GetAllAsync(new EmailConfig { Id = 2 }).Result;
            var setting= _mapper.Map<EmailSettings>(config.FirstOrDefault());
            AppUtility.O.SendMail();
        }
    }
}
