using ApiRequestUtility;
using Dapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using Newtonsoft.Json;
using Paymentgateway.Paytm;
using paytm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Reops
{
    public class GateWayRepo : IGateWay
    {
        private IDapperRepository _dapper;
        protected ICommon _common;
        public GateWayRepo(IDapperRepository dapper, ICommon common)
        {
            _dapper = dapper;
            _common = common;
        }

        public async Task<PaytmJSRequest> IntiatePGTransactionForApp(InitiatePaymentGatewayRequest req)
        {
            PaytmJSRequest response = new PaytmJSRequest();
            try
            {
                var dbparams = new DynamicParameters();
                string responseData = string.Empty;
                string HitURL = string.Empty;
                dbparams.Add("UserID", req.UserID);
                dbparams.Add("Amount", req.Amount);
                InitiatePaymentGatewayResponse inires = await _dapper.InsertAsync<InitiatePaymentGatewayResponse>("proc_InitiatePaymentGatewayTransaction", dbparams, commandType: CommandType.StoredProcedure);

                if (inires.StatusCode == ResponseStatus.Success)
                {
                    var res = new PGModelForRedirection()
                    {
                        URL = inires.URL,
                        Statuscode = (int)inires.StatusCode,
                        PGType = 1,
                        paytmJSRequest = new PaytmJSRequest()
                        {
                            MID = inires.MerchantId,
                            OrderID = inires.OrderID,
                            Amount = req.Amount,
                            CallbackUrl = inires.CallBackUrl,
                            TokenType = "TXN_TOKEN"
                        }
                    };
                    var txnAmount = new Dictionary<string, string> {
                    { "value",req.Amount},
                    { "currency", "INR"}
                };
                    var userInfo = new Dictionary<string, string> {
                    { "custId", "CUST_"+req.UserID}
                };
                    var body = new Dictionary<string, object> {
                    {"requestType", "Payment" },
                    {"mid", res.paytmJSRequest.MID},
                    {"websiteName", "Stage"},
                    {"orderId", res.paytmJSRequest.OrderID },
                    {"txnAmount", req.Amount },
                    {"userInfo", userInfo },
                    { "callbackUrl",res.paytmJSRequest.CallbackUrl},
                };
                    var CHECKSUMHASH = CheckSum.generateSignature(JsonConvert.SerializeObject(body), "hatPG5VUKRJUzhj4");
                    var head = new Dictionary<string, string> {
                    { "signature", CHECKSUMHASH }
                };
                    var requestBody = new Dictionary<string, object> {
                    {"body", body },
                    {"head", head }
                };

                    try
                    {
                        res.Statuscode = 1;
                        res.Msg = "Transaction intiated";
                        string post_data = JsonConvert.SerializeObject(requestBody);
                        HitURL = $"{res.URL}theia/api/v1/initiateTransaction?mid={res.paytmJSRequest.MID}&orderId={res.paytmJSRequest.OrderID}";
                        responseData = AppWebRequest.O.PostJsonDataUsingHWRTLS(HitURL.ToString(), requestBody, head).Result;
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
                        responseData = ex.Message + " | " + responseData;
                    }
                    var callreq2 = new CallBackLog()
                    {
                        Request = HitURL.ToString(),
                        Rdata = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                        Response = responseData,
                        Apiname = "initiateTransaction"
                    };
                    var l = _common.InsertLog(callreq2);
                    response.Amount = res.paytmJSRequest.Amount;
                    response.Token = res.paytmJSRequest.Token;
                    response.OrderID = res.paytmJSRequest.OrderID;
                    response.CallbackUrl = res.paytmJSRequest.CallbackUrl;
                    response.MID = res.paytmJSRequest.MID;
                }
            }
            catch (Exception ex)
            {

            }
            return response ?? new PaytmJSRequest();
        }

        public async Task<PGModelForRedirection> IntiatePGTransactionForWeb(InitiatePaymentGatewayRequest req)
        {
            PaytmJSRequest response = new PaytmJSRequest();

            var dbparams = new DynamicParameters();
            dbparams.Add("UserID", req.UserID);
            dbparams.Add("Amount", req.Amount);
            InitiatePaymentGatewayResponse inires = await _dapper.InsertAsync<InitiatePaymentGatewayResponse>("proc_InitiatePaymentGatewayTransaction", dbparams, commandType: CommandType.StoredProcedure);

            var res = new PGModelForRedirection()
            {
                URL = "https://securegw-stage.paytm.in/",

                Statuscode = 1,
                PGType = 1,
                paytmJSRequest = new PaytmJSRequest()
                {
                    MID = "rZdffo36021582175490",
                    OrderID = inires.OrderID,
                    Amount = inires.Amount,
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
                    Amount = inires.Amount,
                    OrderID = inires.OrderID,
                    TokenType = "TXN_TOKEN"
                };
                var txnAmount = new Dictionary<string, string> {
                    { "value",inires.Amount},
                    { "currency", "INR"}
                };

                var userInfo = new Dictionary<string, string> {
                    { "custId", "CUST_"+req.UserID}
                };
                var body = new Dictionary<string, object> {
                    {"requestType", "Payment" },
                    {"mid",  "rZdffo36021582175490" },
                    {"websiteName", "Stage"},
                    {"orderId", inires.OrderID },
                    {"txnAmount", txnAmount },
                    {"userInfo", userInfo },
                    { "callbackUrl",inires.CallBackUrl},
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
                HitURL.Replace("{ORDER_ID}", inires.OrderID);
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
            catch (Exception ex) { }

            return res;

        }


        public async Task<Response> UpdateGateWayTransaction(UpdatePGTransactionRequest req)
        {
            var res = new Response()
            {
                StatusCode = ResponseStatus.Pending,
                ResponseText = ResponseStatus.Pending.ToString(),
            };
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("OrderID", req.OrderID);
                dbparams.Add("LiveID", req.LiveID);
                dbparams.Add("Amount", req.Amount);
                dbparams.Add("Status", req.Status);
                dbparams.Add("Remark", req.Remark);
                res = await _dapper.InsertAsync<Response>("prco_UpdatePaytmPGTRansaction", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch
            {

            }
            return res;
        }
        public async Task<GatewayRequest> SelectPaymentGateWayDetail()
        {
            var dbparams = new DynamicParameters();
            var res = await _dapper.GetAsync<GatewayRequest>("proc_SelectPaymentGateWayDetail", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }

        public Task<Response> AddAsync(GateWay entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GateWay>> GetAllAsync(GateWay entity = null)
        {
            throw new NotImplementedException();
        }

        public Task<Response<GateWay>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GateWay> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<GateWay>> GetDropdownAsync(GateWay entity)
        {
            throw new NotImplementedException();
        }
    }

}
