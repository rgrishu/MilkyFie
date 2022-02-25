using GenricFrame.AppCode.CustomAttributes;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.Models;
using GenricFrame.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace GenricFrame.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration _config;
        private readonly AppSettings _appSettings;
        private readonly UserManager<AppicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<AppicationUser> _signInManager;
        private IRepository<AppicationUser> _users;
        public AccountController(IConfiguration config, IOptions<AppSettings> appSettings,
            UserManager<AppicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            SignInManager<AppicationUser> signInManager, IRepository<AppicationUser> users)
        {
            _config = config;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _users = users;
        }
        [HttpGet]
        public IActionResult Register()
        {
            
            return View(new RegisterViewModel { IsAdmin = false });
        }

        [HttpPost]
        [ValidateAjaxAttribute]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Response response=new Response();
            response.StatusCode = Status.Failed;
            response.ResponseText ="Registration Failed";
            if (!ModelState.IsValid)
            {
                return Json(response);
            }
            if (string.IsNullOrEmpty(model.RoleName))
            {
                model.RoleName = "Consumer";
            }
            var user = new AppicationUser
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = model.EmailId,
                Email = model.EmailId,
                Role = model.RoleName,
                Name = model.Name,
                Address = model.Address,
                Pincode = model.PinCode,
                PhoneNumber = model.Mobile
            };
            var res = await _userManager.CreateAsync(user, model.Password);
            if (res.Succeeded)
            {
                user = _userManager.FindByEmailAsync(user.Email).Result;
                await _userManager.AddToRoleAsync(user, model.RoleName);
                model.Password = String.Empty;
                model.EmailId = String.Empty;
                ModelState.Clear();
                response.StatusCode = Status.Success;
                response.ResponseText = "Register Successfully";
            }
            else
            {
                foreach (var error in res.Errors)
                {
                    ModelState.TryAddModelError("", error.Description);
                    response.ResponseText = error.Description;
                }
            }
            return Json(response);
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var result = await _signInManager.PasswordSignInAsync(model.MobileNo, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {

                var roles = await _userManager.GetRolesAsync(new AppicationUser { Email = model.MobileNo });
                if (roles != null)
                    if (roles.FirstOrDefault() == "1")
                    {
                        returnUrl = "/Home";
                        return LocalRedirect(returnUrl);
                    }
                    else if (roles.FirstOrDefault() == "3")
                    {
                        returnUrl = "/Consumer";
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View();
                    }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApiLogin(LoginViewModel model)
        {
            var response = new Response<AuthenticateResponse>
            {
                StatusCode = Status.Failed
            };
            var result = await _signInManager.PasswordSignInAsync(model.MobileNo, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                var user = _users.GetDetails(model.MobileNo).Result;
                var token = generateJwtToken(user);
                var authResponse = new AuthenticateResponse(user, token);
                response = new Response<AuthenticateResponse>
                {
                    StatusCode = Status.Success,
                    Result = authResponse
                };
            }
            else
            {
                response.ResponseText = "Authentication Failed";
            }
            return Json(response);
        }

        //[HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();

            return LocalRedirect(returnUrl);

        }
        [Authorize]
        public IActionResult Users()
        {

            var users = _userManager.Users.ToList();

            return View(users);

        }
        [HttpPost]
        public async Task<IActionResult> Users(int id=0)
        {
            var users = _userManager.Users.ToList();
            return PartialView("~/Views/Account/PartialView/_UsersList.cshtml", users);
        }
        /* JWT */
        #region JWT
        //[HttpPost("authenticate")]
        //public IActionResult Authenticate([FromBody] LoginRequest model)
        //{
        //    var response = Authenticate(model.UserName, model.Password);
        //    if (response == null)
        //        return BadRequest(new { message = "Username or password is incorrect" });
        //    return Ok(response);
        //}
        //// helper methods
        //private AuthenticateResponse Authenticate(string userName, string password)
        //{
        //    var user = _users.SingleOrDefault(x => x.UserName == userName && x.PasswordHash == password);

        //    // return null if user not found
        //    if (user == null) return null;

        //    // authentication successful so generate jwt token
        //    var token = generateJwtToken(user);

        //    return new AuthenticateResponse(user, token);
        //}
        private string generateJwtToken(AppicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            /* Claims */
            var claims = new[] {
                new Claim("id", user.UserId.ToString()),
                new Claim("role", user.Role),
                new Claim("userName", user.UserName),
               // new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
               // new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
               // new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")),
               // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            /* End Claims */
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
        /* End */
    }
}