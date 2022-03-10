using AutoMapper;
using Milkyfie.AppCode.CustomAttributes;
using Milkyfie.AppCode.Data;
using Milkyfie.AppCode.Extensions;
using Milkyfie.AppCode.Helper;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
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
using Paymentgateway;
using Paymentgateway.Paytm;

namespace Milkyfie.Controllers
{
    public class GatewayController : Controller
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
        public GatewayController(IOptions<AppSettings> appSettings,
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
        //[HttpGet]
        //[Route("redirect-pg")]
        //public IActionResult PGRedirect(int id, int a, int w, int pg)
        //{
        //    if (_lr.RoleID != Role.Admin && _lr.LoginTypeID == LoginType.ApplicationUser && ApplicationSetting.IsAddMoneyEnable)
        //    {
        //        ILoginML loginML = new LoginML(_accessor, _env);
        //        var _WInfo = loginML.GetWebsiteInfo();
        //        IPaymentGatewayML gatewayML = new PaymentGatewayML(_accessor, _env);

        //         var res = IntiatePGTransactionForWeb(_lr.UserID, a, pg, id, w, _WInfo.WID == 1 ? _WInfo.AbsoluteHost : _WInfo.MainDomain, string.Empty);
              
        //        return View(res);
        //    }
        //    return BadRequest(new { msg = "Unautorised request" });
        //}
        //public GatewayRequest IntiatePGTransactionForWeb(int UserID, decimal Amount, int UPGID, int OID, int WalletID, string Domain, string VPA)
        //{
        //    GatewayResponse res = new GatewayResponse();
        //    var req = new GatewayRequest
        //    {
        //        UserID = UserID,
        //        Amount = Amount,
        //    };
        //    IProcedure proc = new ProcPGatewayTransacrionService(_dal);
        //    var procRes = (PGTransactionResponse)proc.Call(req);
        //    res.Statuscode = procRes.Statuscode;
        //    res.Msg = procRes.Msg;
        //    procRes.VPA = VPA;
        //    if (procRes.Statuscode == ErrorCodes.One)
        //    {
        //        if (procRes.PGID == PaymentGatewayType.PAYTM)
        //        {
        //            procRes.Domain = Domain;
        //            var paytmML = new PaytmML(_dal);
        //            res = paytmML.GeneratePGRequestForWeb(procRes, SavePGTransactionLog);
        //            res.URL = procRes.URL;
        //        }
        //    }

        //    res.TID = procRes.TID;


        //    res.PGType = procRes.PGID;
        //    return res;
        //}



    }
}