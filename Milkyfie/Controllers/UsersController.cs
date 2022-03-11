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
           IRepository<Unit> unit, IRepository<Product> product, IOrder orderschedule, IMapper mapper, IHttpContextAccessor httpContext, IUser users) : base(dapper, category, unit, product, mapper)
        {
            _httpContext = httpContext;
            _user = (Models.ApplicationUser)_httpContext.HttpContext.Items["User"];
            _users = users;
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


    }
}