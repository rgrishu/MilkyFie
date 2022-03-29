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
using System.Linq;

namespace Milkyfie.Controllers
{

    public class UsersController : BaseController
    {

        private IHttpContextAccessor _httpContext;
        // private ApplicationUser _user;
        private IUser _users;


        public UsersController(IDapperRepository dapper, IRepository<Category> category,
           IRepository<Unit> unit, IProduct product, IOrder orderschedule, IMapper mapper, IHttpContextAccessor httpContext, IUser users) : base(dapper, category, unit, product, mapper)
        {
            _httpContext = httpContext;
            //_user = (Models.ApplicationUser)_httpContext.HttpContext.Items["User"];
            _users = users;
        }

        public IActionResult UserDetail()
        {
            ApplicationUser au = new ApplicationUser();
            au.Role = "Consumer";
            return View(au);
        }

        public IActionResult UserDetailFos()
        {
            ApplicationUser au = new ApplicationUser();
            au.Role = "Fos";
            return View("UserDetail", au);
        }

        [Route("UsersDetails")]
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
            return PartialView("~/Views/Users/PartialView/_UserDetailList.cshtml", users);
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
        public async Task<IActionResult> UsersDropdownForFosMap()
        {

            var users = _users.GetUserForFosMApDrpdwn().Result;
            return Json(users);
        }
        [HttpPost]
        public async Task<IActionResult> UserBalance(int id)
        {
            var entity = new ApplicationUser()
            {
                Id = id
            };
            return PartialView("~/Views/Users/PartialView/_AddBalance.cshtml", entity);
        }
        [HttpPost]
        public async Task<IActionResult> AddUserBalance(ApplicationUser entity)
        {
            var userId = User.GetLoggedInUserId<int>();
            entity.UserId = userId.ToString();
            var data = _users.AddAsync(entity).Result;
            return Json(data);
        }

        [Route("Users/DeleteUser")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var data = _users.DeleteAsync(id).Result;
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> FosMap()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> FosMapByUser()
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
        public async Task<IActionResult> FosMappingByUser(int Id, int UserID)
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
            if (UserID == 0)
            {
                res.ResponseText = "Select PinCode!";
                return Json(res);
            }
            var entity = new FOSMapByUser()
            {
                FOSUsers = new ApplicationUser()
                {
                    Id = Id,
                },
                UserID = UserID
            };
            res = _users.FosMappingByUser(entity).Result;
            return Json(res);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFosMappingByUser(int id)
        {
            var entity = new FOSMapByUser()
            {
                FOSMapID = id
            };
            var data = _users.DeleteFosMappingByUser(entity).Result;
            return Json(data);
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
        [HttpPost]
        public async Task<IActionResult> GetMapedFosByUser()
        {
            var data = _users.GetMapedFosByUser().Result;
            return PartialView("PartialView/_FosMapByUserList", data);
        }
        [HttpPost]
        [Route("AdminBalance")]
        public IActionResult AdminBalance()
        {
            return PartialView("~/Views/Home/PartialView/_AdminBalAdd.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> AddAdminBalance(ApplicationUser entity)
        {
            var userId = User.GetLoggedInUserId<int>();
            entity.UserId = userId.ToString();
            entity.Id = userId;
            var data = _users.AddAsync(entity).Result;
            return Json(data);
        }


        [HttpPost]
        [Route("Dashboard")]
        public async Task<IActionResult> LoadDashboard()
        {
            var userId = User.GetLoggedInUserId<int>(); // Specify the type of your UserId;
            var entity = new Dashboard()
            {
                User = new ApplicationUser()
                {
                    Id = userId
                },
            };
            var resp = _users.GetUserDashBoard(entity).Result;
            return PartialView("~/Views/Home/PartialView/_Dashboard.cshtml", resp);
        }
        [HttpPost]
        public async Task<IActionResult> UserForm(string role, int id = 0)
        {
            var rv = new RegisterViewModel()
            {
                IsAdmin = true,
                RoleType = role
            };

            if (id > 0)
            {
                var entity = new ApplicationUser()
                {
                    Id = id
                };
                var users = _users.GetAllAsync(entity).Result;
                if (users != null)
                {
                    var v = users.FirstOrDefault();
                    rv.Name = v.Name;
                    rv.PinCode = v.Pincode;
                    rv.Mobile = v.PhoneNumber;
                    rv.EmailId = v.Email;
                    rv.Address = v.Address;
                    rv.RoleType = v.Role;
                    rv.id = v.Id;
                }

            }
            return PartialView("~/Views/Users/PartialView/_Register.cshtml", rv);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUserDetail(RegisterViewModel model)
        {
            Response response = new Response();
            response.StatusCode = ResponseStatus.Failed;
            response.ResponseText = "Updation Failed";
            if (model == null)
            {
                response.ResponseText = "Invalid Data";
                return Json(response);
            }
            if (model.id == 0)
            {
                response.ResponseText = "Invalid User";
                return Json(response);
            }
            if (string.IsNullOrEmpty(model.Mobile))
            {
                response.ResponseText = "Invalid Mobile";
                return Json(response);
            }
            if (string.IsNullOrEmpty(model.EmailId))
            {
                response.ResponseText = "Invalid Email";
                return Json(response);
            }
            var user = new ApplicationUser
            {
                Id = model.id,
                Email = model.EmailId,
                Name = model.Name,
                Address = model.Address,
                Pincode = model.PinCode,
                PhoneNumber = model.Mobile
            };
            response = await _users.UpdateUserDetail(user);
            return Json(response);
        }


        [HttpPost]
        public IActionResult UserFilter(jsonAOData jsonAOData, UserFilter filters)
        {
            //jsonAOData.param = new { LedgerID = 0, searchText = jsonAOData.search?.value};
            jsonAOData.param = filters;
            var res = (JDataTable<ApplicationUser>)_users.UserFilter(jsonAOData).Result;
            return Json(res);
        }
    }
}