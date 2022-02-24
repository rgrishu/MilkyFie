using GenricFrame.AppCode.CustomAttributes;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenricFrame.Controllers
{
    [ApiController]
    [Route("api/")]

    public class APIController : Controller
    {
        private IUserService _userService;
        private IHttpContextAccessor _httpContext;
        private LoginResponse loginResponse;

        public APIController(IHttpContextAccessor httpContext, IUserService userService)
        {
            _userService = userService;
            _httpContext = httpContext;
            loginResponse = (LoginResponse)_httpContext.HttpContext.Items["User"];
        }



        [HttpPost]
        [Route("Registration")]
        //public IActionResult UserRegistration([FromBody] RegisterViewModel getIntouch)
        public IActionResult UserRegistration()
        {
            
            return Json("");
        }

        [JWTAuthorize]
        [HttpGet(nameof(loginInfo))]
        public IActionResult loginInfo()
        {
            return Ok(loginResponse);
        }
    }
}
