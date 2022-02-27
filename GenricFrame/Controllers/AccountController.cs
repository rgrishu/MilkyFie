using AutoMapper;
using GenricFrame.AppCode.CustomAttributes;
using GenricFrame.AppCode.Data;
using GenricFrame.AppCode.Extensions;
using GenricFrame.AppCode.Helper;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace GenricFrame.Controllers
{
    public class AccountController : Controller
    {
        #region Variables
        //private IConfiguration _config;
        private readonly AppSettings _appSettings;
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationUserManager _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IRepository<ApplicationUser> _users;
        private readonly ILogger<AccountController> _logger;
        private readonly IRepository<EmailConfig> _emailConfig;
        private IMapper _mapper;
        #endregion
        //IConfiguration config
        public AccountController(IOptions<AppSettings> appSettings,
            ApplicationUserManager userManager, RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager, IRepository<ApplicationUser> users,
            ILogger<AccountController> logger, IRepository<EmailConfig> emailConfig, IMapper mapper)
        {
            //_config = config;
            _logger = logger;
            _emailConfig = emailConfig;
            _mapper = mapper;
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
            Response response = new Response();
            response.StatusCode = Status.Failed;
            response.ResponseText = "Registration Failed";
            if (!ModelState.IsValid)
            {
                return Json(response);
            }
            if (string.IsNullOrEmpty(model.RoleName))
            {
                model.RoleName = "Consumer";
            }
            var user = new ApplicationUser
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

        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ReturnUrl = ReturnUrl ?? Url.Content("~/");
            var result = await _signInManager.PasswordSignInAsync(model.MobileNo, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {

                var roles = await _userManager.GetRolesAsync(new ApplicationUser { Email = model.MobileNo });
                _userManager.AddToRoleAsync(new ApplicationUser { Email = model.MobileNo }, roles?.FirstOrDefault());
                if (roles != null)
                    if (roles.FirstOrDefault().Equals("1") || roles.FirstOrDefault().Equals("admin",StringComparison.OrdinalIgnoreCase))
                    {
                        ReturnUrl = ReturnUrl?.Trim() == "/" ? "/Home" : ReturnUrl;
                        return LocalRedirect(ReturnUrl);
                    }
                    else if (roles.FirstOrDefault().Equals("3") || roles.FirstOrDefault().Equals("consumer",StringComparison.OrdinalIgnoreCase))
                    {
                        ReturnUrl = ReturnUrl?.Trim() == "/" ? "/Consumer" : ReturnUrl;
                        return LocalRedirect(ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View();
                    }
            }
            else if (result.IsLockedOut)
            {
                var Scheme = Request.Scheme;
                var forgotPassLink = Url.Action(nameof(ForgotPassword), "Account", new { }, Request.Scheme);
                var content = string.Format("Your account is locked out, to reset your password, please click this link: {0}", forgotPassLink);
                //var message = new Message(new string[] { model.MobileNo }, "Locked out account information", content, null);
                var config = _emailConfig.GetAllAsync(new EmailConfig { Id = 2 }).Result;
                var setting = _mapper.Map<EmailSettings>(config.FirstOrDefault());
                setting.Body = content;
                setting.Subject = "Locked out account information";
                var _ = AppUtility.O.SendMail(setting);
            }
        Finish:
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApiLogin(LoginViewModel model)
        {
            var response = new Response<AuthenticateResponse>
            {
                StatusCode = Status.Failed,
                ResponseText = "Invalid Login Attempt"
            };
            var result = await _signInManager.PasswordSignInAsync(model.MobileNo, model.Password, false, true);
            if (result.Succeeded)
            {
                var user = _users.GetDetails(model.MobileNo).Result;
                var token = generateJwtToken(user);
                var authResponse = new AuthenticateResponse(user, token);
                response = new Response<AuthenticateResponse>
                {
                    StatusCode = Status.Success,
                    ResponseText = Status.Success.ToString(),
                    Result = authResponse
                };
                goto Finish;
            }
        Finish:
            return Json(response);
        }

        [HttpPost]
        public IActionResult ForgotPassword()
        {
            return Json("");
        }

        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();

            return LocalRedirect(returnUrl);

        }
        [Authorize]
        public IActionResult Users()
        {
            var _ = _userManager.FindByMobileNoAsync("").Result;
            var users = _userManager.Users.ToList();
            return View(users);

        }
        [HttpPost]
        public async Task<IActionResult> Users(int id = 0)
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
        private string generateJwtToken(ApplicationUser user)
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