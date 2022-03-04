using Milkyfie.AppCode.CustomAttributes;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Milkyfie.AppCode.Extensions;

namespace Milkyfie.Controllers
{
   
    public class UsersController : Controller
    {
        private IUserService _userService;
        private IHttpContextAccessor _httpContext;
        private ApplicationUser _user;
        private IRepository<ApplicationUser> _users;

        public UsersController(IHttpContextAccessor httpContext, IUserService userService, IRepository<ApplicationUser> users)
        {
            _userService = userService;
            _httpContext = httpContext;
            _user = (Models.ApplicationUser)_httpContext.HttpContext.Items["User"];
            _users = users;
        }
        //[JWTAuthorize]
        //[HttpGet("getall")]
        //public IActionResult GetAll()
        //{
        //    var users = _userService.GetAll();
        //    return Ok(users);
        //}
        [HttpPost]
        public async Task<IActionResult> UserBalance(int id)
        {
            var entity = new ApplicationUser()
            {
                Id = id
            };
            return PartialView("~/Views/Account/PartialView/_AddBalance.cshtml", entity);

        }

        [HttpPost]
        public async Task<IActionResult> AddUserBalance(ApplicationUser entity)
        {
            var userId = User.GetLoggedInUserId<int>();
            entity.UserId = userId.ToString();
            var data = _users.AddAsync(entity).Result;
            return Json(data);
        }

    }
}