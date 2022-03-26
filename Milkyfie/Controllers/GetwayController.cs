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
using System.Collections.Generic;
using Newtonsoft.Json;
using paytm;
using ApiRequestUtility;

namespace Milkyfie.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
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


        [HttpGet]
        [Route("redirect-pg")]
        public IActionResult PGRedirect(int id, int a, int w, int pg)
        {
            //    ILoginML loginML = new LoginML(_accessor, _env);
            //    var _WInfo = loginML.GetWebsiteInfo();
            //    IPaymentGatewayML gatewayML = new PaymentGatewayML(_accessor, _env);

            //    var res = IntiatePGTransactionForWeb(_lr.UserID, a, pg, id, w, _WInfo.WID == 1 ? _WInfo.AbsoluteHost : _WInfo.MainDomain, string.Empty);

            //    return View(res);

            //return BadRequest(new { msg = "Unautorised request" });
            var res = new PGModelForRedirection()
            {
                URL= "https://securegw-stage.paytm.in/",

                Statuscode =1,
                PGType=1,
                paytmJSRequest = new PaytmJSRequest()
                {
                    MID= "rZdffo36021582175490",
                    OrderID="1234567",
                    Amount="100",
                }
            };
            var paytmPGRequest = new PaytmPGRequest();
            try
            {
                res.Statuscode = 1;
                res.Msg = "Transaction intiated";
                res.paytmJSRequest = new PaytmJSRequest
                {
                    MID = "rZdffo36021582175490",
                    Amount = "100",
                    OrderID = "1234567",
                    TokenType = "TXN_TOKEN"
                };
                var txnAmount = new Dictionary<string, string> {
                    { "value","100"},
                    { "currency", "INR"}
                };

                var userInfo = new Dictionary<string, string> {
                    { "custId", "CUST_"+1}
                };
                var body = new Dictionary<string, object> {
                    {"requestType", "Payment" },
                    {"mid",  "rZdffo36021582175490" },
                    {"websiteName", "Stage"},
                    {"orderId", "1234567" },
                    {"txnAmount", txnAmount },
                    {"userInfo", userInfo },
                    { "callbackUrl","http://localhost:53104/"},
                };
                res.paytmJSRequest.CallbackUrl = Convert.ToString(body["callbackUrl"]);
                paytmPGRequest.CHECKSUMHASH = CheckSum.generateSignature(JsonConvert.SerializeObject(body), "hatPG5VUKRJUzhj4");
                var head = new Dictionary<string, string> {
                    { "signature", paytmPGRequest.CHECKSUMHASH }
                };
                var requestBody = new Dictionary<string, object> {
                    {"body", body },
                    {"head", head }
                };
                string post_data = JsonConvert.SerializeObject(requestBody);
                StringBuilder HitURL = new StringBuilder("{HOST}theia/api/v1/initiateTransaction?mid={MID}&orderId={ORDER_ID}");
                HitURL.Replace("{HOST}", "https://securegw-stage.paytm.in/");
                HitURL.Replace("{MID}", "rZdffo36021582175490");
                HitURL.Replace("{ORDER_ID}", "1234567");
                var responseData = AppWebRequest.O.PostJsonDataUsingHWRTLS(HitURL.ToString(), requestBody, head).Result;
                if (!string.IsNullOrEmpty(responseData))
                {
                    var apiResp = JsonConvert.DeserializeObject<PaytmTokenResponse>(responseData);
                    if (apiResp.body != null)
                    {
                        if (apiResp.body.resultInfo.resultCode.Equals("0000"))
                        {
                            res.paytmJSRequest.Token = apiResp.body.txnToken;
                        }
                    }
                }

            }
            catch(Exception ex) { }

            return View(res);
        }
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


        public PGModelForRedirection GeneratePGRequestForJS()
        {
            var res = new PGModelForRedirection
            {
                Statuscode = -1,
                Msg = "Failed"
            };
            var paytmPGRequest = new PaytmPGRequest();
            try
            {
                res.Statuscode = 1;
                res.Msg = "Transaction intiated";
                res.paytmJSRequest = new PaytmJSRequest
                {
                    MID = "rZdffo36021582175490",
                    Amount = "100",
                    OrderID = "1234567",
                    TokenType = "TXN_TOKEN"
                };
                var txnAmount = new Dictionary<string, string> {
                    { "value","100"},
                    { "currency", "INR"}
                };

                var userInfo = new Dictionary<string, string> {
                    { "custId", "CUST_"+1}
                };
                var body = new Dictionary<string, object> {
                    {"requestType", "Payment" },
                    {"mid",  "rZdffo36021582175490" },
                    {"websiteName", "Stage"},
                    {"orderId", "1234567" },
                    {"txnAmount", txnAmount },
                    {"userInfo", userInfo },
                    { "callbackUrl","http://localhost:53104/"},
                };
                res.paytmJSRequest.CallbackUrl = Convert.ToString(body["callbackUrl"]);
                paytmPGRequest.CHECKSUMHASH = CheckSum.generateSignature(JsonConvert.SerializeObject(body), "hatPG5VUKRJUzhj4");
                var head = new Dictionary<string, string> {
                    { "signature", paytmPGRequest.CHECKSUMHASH }
                };
                var requestBody = new Dictionary<string, object> {
                    {"body", body },
                    {"head", head }
                };
                string post_data = JsonConvert.SerializeObject(requestBody);
                StringBuilder HitURL = new StringBuilder("{HOST}theia/api/v1/initiateTransaction?mid={MID}&orderId={ORDER_ID}");
                HitURL.Replace("{HOST}", "https://securegw-stage.paytm.in/");
                HitURL.Replace("{MID}", "rZdffo36021582175490");
                HitURL.Replace("{ORDER_ID}", "1234567");
                var responseData = AppWebRequest.O.PostJsonDataUsingHWRTLS(HitURL.ToString(), requestBody, head).Result;
                if (!string.IsNullOrEmpty(responseData))
                {
                    var apiResp = JsonConvert.DeserializeObject<PaytmTokenResponse>(responseData);
                    if (apiResp.body != null)
                    {
                        if (apiResp.body.resultInfo.resultCode.Equals("0000"))
                        {
                            res.paytmJSRequest.Token = apiResp.body.txnToken;
                        }
                    }
                }
             
            }
            catch (Exception ex)
            {
                
            }

            return res;
        }
    }
}