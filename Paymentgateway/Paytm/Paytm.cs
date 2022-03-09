using System.Collections.Generic;
using paytm;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Paymentgateway.Paytm;
using ApiRequestUtility;

namespace Paytm
{
    public partial class Paytm
    {
        public GatewayResponse GeneratePGRequestForWeb(GatewayRequest request)
        {
            var res = new GatewayResponse
            {
                Statuscode = -1,
                ResponseText = "Something went wrong"
            };
            var paytmPGRequest = new PaytmPGRequest();
            try
            {
                var KeyVals = new Dictionary<string, string>
                {
                    { nameof(paytmPGRequest.MID), request.MerchantID },
                    { nameof(paytmPGRequest.WEBSITE), request.ENVCode },
                    { nameof(paytmPGRequest.INDUSTRY_TYPE_ID), request.IndustryType },
                    { nameof(paytmPGRequest.CHANNEL_ID), "WEB" },
                    { nameof(paytmPGRequest.ORDER_ID),  request.TID.ToString()},
                    { nameof(paytmPGRequest.CUST_ID),  request.UserID.ToString()},
                    { nameof(paytmPGRequest.MOBILE_NO),  request.MobileNo},
                    { nameof(paytmPGRequest.EMAIL),  request.EmailID},
                    { nameof(paytmPGRequest.TXN_AMOUNT),  request.Amount.ToString()},
                    { nameof(paytmPGRequest.CALLBACK_URL),  request.Domain+"/PGCallback/Paytm"}
                };
                paytmPGRequest.CHECKSUMHASH = CheckSum.generateCheckSum(request.MerchantKey, KeyVals);
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public GatewayResponse GeneratePGRequestForJS(GatewayRequest request)
        {
            var res = new GatewayResponse<PaytmJSRequest>
            {
                Statuscode = -1,
                ResponseText = "Something went wrong"
            };
            var paytmPGRequest = new PaytmPGRequest();
            try
            {
                res.Data = new PaytmJSRequest
                {
                    MID = request.MerchantID,
                    Amount = request.Amount.ToString(),
                    OrderID = request.TID.ToString(),
                    TokenType = "TXN_TOKEN"
                };
                var txnAmount = new Dictionary<string, string> {
                    { "value", request.Amount.ToString()},
                    { "currency", "INR"}
                };

                var userInfo = new Dictionary<string, string> {
                    { "custId", "CUST_"+request.UserID}
                };
                var body = new Dictionary<string, object> {
                    {"requestType", "Payment" },
                    {"mid",  request.MerchantID },
                    {"websiteName", request.ENVCode},
                    {"orderId", request.TID.ToString() },
                    {"txnAmount", txnAmount },
                    {"userInfo", userInfo },
                    { "callbackUrl", request.Domain+"/PGCallback/Paytm"},
                };
                res.Data.CallbackUrl = Convert.ToString(body["callbackUrl"]);
                paytmPGRequest.CHECKSUMHASH = CheckSum.generateSignature(JsonConvert.SerializeObject(body), request.MerchantKey);
                var head = new Dictionary<string, string> {
                    { "signature", paytmPGRequest.CHECKSUMHASH }
                };
                var requestBody = new Dictionary<string, object> {
                    {"body", body },
                    {"head", head }
                };
                string post_data = JsonConvert.SerializeObject(requestBody);
                StringBuilder HitURL = new StringBuilder("{HOST}theia/api/v1/initiateTransaction?mid={MID}&orderId={ORDER_ID}");
                HitURL.Replace("{HOST}", request.URL);
                HitURL.Replace("{MID}", request.MerchantID);
                HitURL.Replace("{ORDER_ID}", request.TID.ToString());
                var responseData = AppWebRequest.O.PostJsonDataUsingHWRTLS(HitURL.ToString(), requestBody, head).Result;
                if (!string.IsNullOrEmpty(responseData))
                {
                    var apiResp = JsonConvert.DeserializeObject<PaytmTokenResponse>(responseData);
                    if (apiResp.body != null)
                    {
                        if (apiResp.body.resultInfo.resultCode.Equals("0000"))
                        {
                            res.Data.Token = apiResp.body.txnToken;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        public void GeneratePGRequestForJSApp(GatewayRequest request, GatewayResponse<PaytmPGRequest> res)
        {
            var paytmPGRequest = new PaytmPGRequest
            {
                MID = request.MerchantID,
                WEBSITE = request.ENVCode,
                INDUSTRY_TYPE_ID = request.IndustryType,
                CHANNEL_ID = "WAP",
                ORDER_ID = request.TID.ToString(),
                CUST_ID = request.UserID.ToString(),
                MOBILE_NO = request.MobileNo,
                EMAIL = request.EmailID,
                TXN_AMOUNT = request.Amount.ToString(),
                CALLBACK_URL = request.Domain + "/PGCallback/Paytm"
            };
            var KeyVals = new Dictionary<string, string>
            {
                { nameof(paytmPGRequest.MID), paytmPGRequest.MID },
                { nameof(paytmPGRequest.WEBSITE),  paytmPGRequest.WEBSITE},
                { nameof(paytmPGRequest.INDUSTRY_TYPE_ID),  paytmPGRequest.INDUSTRY_TYPE_ID},
                { nameof(paytmPGRequest.CHANNEL_ID), paytmPGRequest.CHANNEL_ID },
                { nameof(paytmPGRequest.ORDER_ID),  paytmPGRequest.ORDER_ID},
                { nameof(paytmPGRequest.CUST_ID), paytmPGRequest.CUST_ID },
                { nameof(paytmPGRequest.MOBILE_NO), paytmPGRequest.MOBILE_NO },
                { nameof(paytmPGRequest.EMAIL), paytmPGRequest.EMAIL },
                { nameof(paytmPGRequest.TXN_AMOUNT), paytmPGRequest.TXN_AMOUNT },
                { nameof(paytmPGRequest.CALLBACK_URL), paytmPGRequest.CALLBACK_URL }
            };
            var txnAmount = new Dictionary<string, string> {
                    { "value", request.Amount.ToString()},
                    { "currency", "INR"}
                };

            var userInfo = new Dictionary<string, string> {
                    { "custId", "CUST_"+request.UserID}
                };
            var body = new Dictionary<string, object> {
                    {"requestType", "Payment" },
                    {"mid",  request.MerchantID },
                    {"websiteName", request.ENVCode},
                    {"orderId", request.TID.ToString() },
                    {"txnAmount", txnAmount },
                    {"userInfo", userInfo },
                    {"callbackUrl", request.Domain+"/PGCallback/Paytm"},
                };

            paytmPGRequest.CHECKSUMHASH = CheckSum.generateSignature(JsonConvert.SerializeObject(body), request.MerchantKey);
            var head = new Dictionary<string, string> {
                    { "signature", paytmPGRequest.CHECKSUMHASH }
                };
            var requestBody = new Dictionary<string, object> {
                    {"body", body },
                    {"head", head }
                };
            string post_data = JsonConvert.SerializeObject(requestBody);
            StringBuilder HitURL = new StringBuilder("{HOST}theia/api/v1/initiateTransaction?mid={MID}&orderId={ORDER_ID}");
            HitURL.Replace("{HOST}", request.URL);
            HitURL.Replace("{MID}", request.MerchantID);
            HitURL.Replace("{ORDER_ID}", request.TID.ToString());
            var responseData = AppWebRequest.O.PostJsonDataUsingHWRTLS(HitURL.ToString(), requestBody, head).Result;
            if (!string.IsNullOrEmpty(responseData))
            {
                var apiResp = JsonConvert.DeserializeObject<PaytmTokenResponse>(responseData);
                if (apiResp.body != null)
                {
                    if (apiResp.body.resultInfo.resultCode.Equals("0000"))
                    {
                        res.Data.Token = apiResp.body.txnToken;
                    }
                }
            }
            res.Data = paytmPGRequest;
        }
        public void GeneratePGRequestForApp(GatewayRequest request, GatewayResponse<PaytmPGRequest> res)
        {
            var paytmPGRequest = new PaytmPGRequest
            {
                MID = request.MerchantID,
                WEBSITE = request.ENVCode,
                INDUSTRY_TYPE_ID = request.IndustryType,
                CHANNEL_ID = "WAP",
                ORDER_ID = request.TID.ToString(),
                CUST_ID = request.UserID.ToString(),
                MOBILE_NO = request.MobileNo,
                EMAIL = request.EmailID,
                TXN_AMOUNT = request.Amount.ToString(),
                CALLBACK_URL = "https://securegw.paytm.in/theia/paytmCallback?ORDER_ID=" + request.TID.ToString()
            };
            var KeyVals = new Dictionary<string, string>
            {
                { nameof(paytmPGRequest.MID), paytmPGRequest.MID },
                { nameof(paytmPGRequest.WEBSITE),  paytmPGRequest.WEBSITE},
                { nameof(paytmPGRequest.INDUSTRY_TYPE_ID),  paytmPGRequest.INDUSTRY_TYPE_ID},
                { nameof(paytmPGRequest.CHANNEL_ID), paytmPGRequest.CHANNEL_ID },
                { nameof(paytmPGRequest.ORDER_ID),  paytmPGRequest.ORDER_ID},
                { nameof(paytmPGRequest.CUST_ID), paytmPGRequest.CUST_ID },
                { nameof(paytmPGRequest.MOBILE_NO), paytmPGRequest.MOBILE_NO },
                { nameof(paytmPGRequest.EMAIL), paytmPGRequest.EMAIL },
                { nameof(paytmPGRequest.TXN_AMOUNT), paytmPGRequest.TXN_AMOUNT },
                { nameof(paytmPGRequest.CALLBACK_URL), paytmPGRequest.CALLBACK_URL }
            };
            paytmPGRequest.CHECKSUMHASH = CheckSum.generateCheckSum(request.MerchantKey, KeyVals);
            res.Data = paytmPGRequest;
        }

        public PaytmPGResponse StatusCheckPG(GatewayRequest request)
        {
            var payresp = new PaytmPGResponse();
            var res = new GatewayResponse
            {
                Statuscode = -1,
                ResponseText = "Something went wrong"
            };
            var paytmPGRequest = new PaytmPGRequest();
            string paytmresp = string.Empty;
            try
            {
                var KeyVals = new Dictionary<string, string>
                {
                    { nameof(paytmPGRequest.MID), request.MerchantID },
                    { nameof(paytmPGRequest.ORDER_ID), request.TID.ToString() }
                };
                paytmPGRequest.CHECKSUMHASH = CheckSum.generateCheckSum(request.MerchantKey, KeyVals);
                KeyVals.Add(nameof(paytmPGRequest.CHECKSUMHASH), paytmPGRequest.CHECKSUMHASH);
                paytmresp = AppWebRequest.O.PostJsonDataUsingHWR(request.StatusCheckURL, KeyVals);
                if (!string.IsNullOrEmpty(paytmresp))
                {
                    payresp = JsonConvert.DeserializeObject<PaytmPGResponse>(paytmresp);
                }
            }
            catch (Exception ex)
            {

            }
            return payresp;
        }

        public PaytmPGResponse StatusCheckPGJS(GatewayRequest request)
        {
            var payresp = new PaytmPGResponse();
            var res = new GatewayResponse
            {
                Statuscode = -1,
                ResponseText = "Something went wrong"
            };
            var paytmPGRequest = new PaytmPGRequest();
            string paytmresp = string.Empty;

            try
            {
                var body = new Dictionary<string, string> {
                    {"mid",  request.MerchantID },
                    {"orderId", request.TID.ToString() }
                };

                paytmPGRequest.CHECKSUMHASH = CheckSum.generateSignature(JsonConvert.SerializeObject(body), request.MerchantKey);
                var head = new Dictionary<string, string> {
                    { "signature", paytmPGRequest.CHECKSUMHASH }
                };
                var requestBody = new Dictionary<string, object> {
                    {"body", body },
                    {"head", head }
                };
                string post_data = JsonConvert.SerializeObject(requestBody);
               
                paytmresp = AppWebRequest.O.PostJsonDataUsingHWRTLS(request.StatusCheckURL, requestBody, head).Result;
                if (!string.IsNullOrEmpty(paytmresp))
                {
                    var payrespTemp = JsonConvert.DeserializeObject<PayTMJSPGResponse>(paytmresp);
                    if (payrespTemp != null)
                    {
                        if (payrespTemp.body != null)
                        {
                            payresp.BANKNAME = payrespTemp.body.bankName;
                            payresp.BANKTXNID = payrespTemp.body.bankTxnId;
                            payresp.CHECKSUMHASH = payrespTemp.head.signature;
                            payresp.GATEWAYNAME = payrespTemp.body.gatewayName;
                            payresp.ORDERID = payrespTemp.body.orderId;
                            payresp.STATUS = payrespTemp.body.resultInfo.resultStatus;
                            payresp.TXNAMOUNT = payrespTemp.body.txnAmount;
                            payresp.TXNID = payrespTemp.body.txnId;
                            payresp.RESPMSG = payrespTemp.body.resultInfo.resultMsg;
                            payresp.RESPCODE = payrespTemp.body.resultInfo.resultCode;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return payresp;
        }
    }
}
