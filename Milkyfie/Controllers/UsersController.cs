using Milkyfie.AppCode.CustomAttributes;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Milkyfie.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IHttpContextAccessor _httpContext;
        private ApplicationUser _user;

        public UsersController(IHttpContextAccessor httpContext, IUserService userService)
        {
            _userService = userService;
            _httpContext = httpContext;
            _user = (Models.ApplicationUser)_httpContext.HttpContext.Items["User"];
        }

        //[HttpPost("authenticate")]
        //public IActionResult Authenticate(LoginRequest model)
        //{
        //    var response = _userService.Authenticate(model);
        //    if (response == null)
        //        return BadRequest(new { message = "Username or password is incorrect" });
        //    return Ok(response);
        //}

        //[Authorize]
        [JWTAuthorize]
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}