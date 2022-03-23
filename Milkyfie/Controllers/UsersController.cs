using Milkyfie.AppCode.CustomAttributes;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Milkyfie.AppCode.Extensions;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Reops.Entities;
using AutoMapper;

namespace Milkyfie.Controllers
{

    public class UsersController : BaseController
    {

        private IHttpContextAccessor _httpContext;
        private ApplicationUser _user;
        private IUser _users;


        public UsersController(IDapperRepository dapper, IRepository<Category> category,
           IRepository<Unit> unit, IProduct product, IOrder orderschedule, IMapper mapper, IHttpContextAccessor httpContext, IUser users) : base(dapper, category, unit, product, mapper)
        {
            _httpContext = httpContext;
            _user = (Models.ApplicationUser)_httpContext.HttpContext.Items["User"];
            _users = users;
        }
        [Route("Account/Users/{role}")]
        public IActionResult Users(string role)
        {
            ApplicationUser au = new ApplicationUser();
            au.Role = role;
            return View("Users", au);
        }

        [HttpPost]
        public async Task<IActionResult> UsersDetails(string role)
        {
            if (role.Trim() != "Fos" && role.Trim() != "Consumer")
            {
                role = string.Empty;
            }
            var users = _users.GetAllAsync().Result;
            if (users.Count() > 0)
            {
                users = users.Where(x => x.Role == role);
            }
            return PartialView("~/Views/Account/PartialView/_UsersList.cshtml", users);
        }


        [HttpPost]
        public async Task<IActionResult> UsersDropdown(string role)
        {

            var users = _users.GetAllAsync().Result;
            if (users.Count() > 0)
            {
                users = users.Where(x => x.Role == (role ?? "Consumer"));
            }
            return Json(users);
        }
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
        [HttpGet]
        public async Task<IActionResult> FosMap()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FosMapping(int Id, int PincodeID)
        {
            var res = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            if (Id == 0)
            {
                res.ResponseText = "Select FOS!";
                return Json(res);
            }
            if (PincodeID == 0)
            {
                res.ResponseText = "Select PinCode!";
                return Json(res);
            }
            var entity = new FOSMap()
            {
                Users = new ApplicationUser()
                {
                    Id = Id,
                },
                pincode = new Pincode()
                {
                    PincodeID = PincodeID
                },
            };
            res = _users.FosMapping(entity).Result;
            return Json(res);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFosMapping(int id)
        {
            var entity = new FOSMap()
            {
                FOSMapID = id
            };
            var data = _users.DeleteFosMapping(entity).Result;
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> GetMapedFos()
        {
            var data = _users.GetMapedFos().Result;
            return PartialView("PartialView/_FosMapList", data);
        }
    }
}