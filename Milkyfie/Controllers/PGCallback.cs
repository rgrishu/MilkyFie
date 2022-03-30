
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Milkyfie.AppCode.CustomAttributes;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using Newtonsoft.Json;
using Paymentgateway.Paytm;
using Paytm;

using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Milkyfie.Controllers
{
    public class PGCallbackController : Controller
    {
        private IDapperRepository _dapper;
        protected ICommon _common;
        protected IGateWay _gateWay;
        public PGCallbackController(IDapperRepository dapper, ICommon common, IGateWay gateWay)
        {
            _dapper = dapper;
            _common = common;
            _gateWay = gateWay;
        }

        [HttpPost]
        public async Task<IActionResult> Paytm([FromForm] PaytmPGResponse paytmPGResponse)
        {
            var TID = 0;
            var TransactionID = string.Empty;
            if (Validate.O.IsNumeric(paytmPGResponse.ORDERID ?? string.Empty))
            {
                TID = Convert.ToInt32(paytmPGResponse.ORDERID);
            }
            var callbackreq = new CallBackLog()
            {
                Request = "PaytmCallback",
                Rdata = (paytmPGResponse != null ? JsonConvert.SerializeObject(paytmPGResponse) : string.Empty),
                Response = TID + "" + TransactionID,
                Apiname = "PaytmCallback"
            };
            var l = _common.InsertLog(callbackreq);

            var pResp = UpdateFromPayTMCallback(paytmPGResponse);

           // var FLYINGARROW = string.Format("http://milkyfie.in/PGCallback/CommonPG");
            var commonPGResp = new CommonPGResponse
            {
                TID = paytmPGResponse.ORDERID ?? string.Empty,
                Amount = paytmPGResponse.TXNAMOUNT ?? string.Empty,
                TransactionID = paytmPGResponse.TXNID ?? string.Empty,
                status = pResp.StatusCode.ToString(),
                reason = pResp.ResponseText ?? string.Empty
            };
           
                var html = new StringBuilder(@"<html><head><script>
                                (()=>{
                                        var obj={TID:""{TID}"",Amount:""{Amount}"",TransactionID:""{TransactionID}"",statuscode:""{status}"",reason:""{reason}"",origin:""addMoney""}
                                        localStorage.setItem('obj', JSON.stringify(obj));
                                        window.close()
                                   })();</script></head><body><h6>Redirect to site.....</h6></body></html>");
                html.Replace("{TID}", commonPGResp.TID);
                html.Replace("{Amount}", commonPGResp.Amount);
                html.Replace("{TransactionID}", commonPGResp.TransactionID);
                html.Replace("{status}", commonPGResp.status);
                html.Replace("{reason}", commonPGResp.reason);
                return Content(html.ToString(), contentType: "text/html; charset=utf-8");
            
//            else
//            {
//                var html = new StringBuilder(@"<!DOCTYPE html>
//<html>
//<body>
//    <form action=""{FLYINGARROW}"" method=""post"" onload=""document.forms[0].submit()"">
//        <input type=""hidden"" id=""TID"" name=""TID"" value=""{TID}""><br>
//        <input type=""hidden"" id=""Amount"" name=""Amount"" value=""{Amount}"">
//        <input type=""hidden"" id=""TransactionID"" name=""TransactionID"" value=""{TransactionID}""><br>
//        <input type=""hidden"" id=""status"" name=""status"" value=""{status}""><br>
//        <input type=""hidden"" id=""reason"" name=""reason"" value=""{reason}""><br>
//    </form>
//</body>
//</html>");
//                html.Replace("{FLYINGARROW}", FLYINGARROW);
//                html.Replace("{TID}", commonPGResp.TID);
//                html.Replace("{Amount}", commonPGResp.Amount);
//                html.Replace("{TransactionID}", commonPGResp.TransactionID);
//                html.Replace("{status}", commonPGResp.status);
//                html.Replace("{reason}", commonPGResp.reason);

//                return Content(html.ToString(), contentType: "text/html; charset=utf-8");
//            }

            // return View("PGConfirmation", Is.Statuscode == ErrorCodes.One);
        }

        public Response UpdateFromPayTMCallback(PaytmPGResponse paytmPGResponse)
        {
            var res = new Response
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = "Invalid update request"
            };
            var TID = 0;
            if (Validate.O.IsNumeric(paytmPGResponse.ORDERID ?? string.Empty))
            {
                TID = Convert.ToInt32(paytmPGResponse.ORDERID);
            }
            var domain = string.Empty;
            var WID = 0;
            try
            {
                var statusResp = new PaytmPGResponse();
                //statusResp = PaytmML.StatusCheckPGJS(ToMatchData, SavePGTransactionLog);
                GatewayRequest request = new GatewayRequest();
                request = _gateWay.SelectPaymentGateWayDetail().Result;
                request.TID = TID;
                Paytm.Paytm pm = new Paytm.Paytm();
                statusResp = pm.StatusCheckPGJS(request);
                if (statusResp.STATUS != null)
                {
                    if (statusResp.STATUS.ToUpper().Equals("PENDING"))
                    {
                        StringBuilder sb = new StringBuilder("Your Order {TID} for Amount {AMOUNT} is awaited from Bank Side We will Update once get Response From Bank.");
                        sb.Replace("{TID}", paytmPGResponse.ORDERID);
                        sb.Replace("{AMOUNT}", paytmPGResponse.TXNAMOUNT);
                        res.ResponseText = sb.ToString();
                        return res;
                    }
                    if (statusResp.STATUS == PAYTMResponseType.FAILED)
                    {
                        paytmPGResponse.STATUS = statusResp.STATUS;
                    }
                    if (paytmPGResponse.STATUS != statusResp.STATUS || paytmPGResponse.TXNAMOUNT != statusResp.TXNAMOUNT)
                    {
                        return res;
                    }
                }
                else
                {
                    return res;
                }
            }
            catch (Exception)
            {
                return res;
            }

            var req = new UpdatePGTransactionRequest
            {

                OrderID = paytmPGResponse.ORDERID,
                VendorID = paytmPGResponse.TXNID,
                LiveID = paytmPGResponse.BANKTXNID,
                Remark = paytmPGResponse.RESPMSG,
                Amount= paytmPGResponse.TXNAMOUNT
            };
            req.Status = (int)Status.Pending;
            if (paytmPGResponse.STATUS == PAYTMResponseType.SUCCESS)
            {
                req.Status = (int)Status.Success;

            }
            if (paytmPGResponse.STATUS == PAYTMResponseType.FAILED)
            {
                req.Status = (int)Status.Failed;
            }
            Response resp = _gateWay.UpdateGateWayTransaction(req).Result;
            res.StatusCode = resp.StatusCode;
            res.ResponseText = resp.ResponseText;
            return res;
        }

        [HttpPost]
        public async Task<IActionResult> CommonPG([FromForm] CommonPGResponse commonPGResp)
        {
            StringBuilder html = new StringBuilder(@"<html><head><script>
                                (()=>{
                                        var obj={TID:""{TID}"",Amount:""{Amount}"",TransactionID:""{TransactionID}"",statuscode:""{status}"",reason:""{reason}"",origin:""addMoney""}
                                        localStorage.setItem('obj', JSON.stringify(obj));
                                        window.close()
                                   })();</script></head><body><h6>Redirect to site.....</h6></body></html>");
            html.Replace("{TID}", commonPGResp.TID);
            html.Replace("{Amount}", commonPGResp.Amount);
            html.Replace("{TransactionID}", commonPGResp.TransactionID);
            html.Replace("{status}", commonPGResp.status);
            html.Replace("{reason}", commonPGResp.reason);
            return Content(html.ToString(), contentType: "text/html; charset=utf-8");
        }
    }
}
