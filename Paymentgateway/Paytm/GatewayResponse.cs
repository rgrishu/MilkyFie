using System;
using System.Collections.Generic;
using System.Text;

namespace Paymentgateway.Paytm
{
    public class GatewayResponse
    {
        public int PGType { get; set; }
        public int TID { get; set; }
        public int Statuscode { get; set; }
        public string ResponseText { get; set; }
        public string URL { get; set; }
    }

    public class GatewayResponse<T> : GatewayResponse
    {
        public T Data { get; set; }
    }
    public class PaytmTokenResponse
    {
        public PaytmHead head { get; set; }
        public PaytmBody body { get; set; }
    }
    public class PaytmHead
    {
        public string responseTimestamp { get; set; }
        public string version { get; set; }
        public string clientId { get; set; }
        public string signature { get; set; }
    }
    public class PaytmBody
    {
        public string txnToken { get; set; }
        public bool isPromoCodeValid { get; set; }
        public bool authenticated { get; set; }
        public PaytmResultInfo resultInfo { get; set; }
    }
    public class PaytmResultInfo
    {
        public string resultStatus { get; set; }
        public string resultCode { get; set; }
        public string resultMsg { get; set; }
    }
    public class PaytmPGRequest
    {
        public string MID { get; set; }
        public string ORDER_ID { get; set; }
        public string CUST_ID { get; set; }
        public string TXN_AMOUNT { get; set; }
        public string CHANNEL_ID { get; set; }
        public string WEBSITE { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL { get; set; }
        public string INDUSTRY_TYPE_ID { get; set; }
        public string CALLBACK_URL { get; set; }
        public string MERC_UNQ_REF { get; set; }
        public string PAYMENT_MODE_ONLY { get; set; }
        public string AUTH_MODE { get; set; }
        public string PAYMENT_TYPE_ID { get; set; }
        public string BANK_CODE { get; set; }
        public string CHECKSUMHASH { get; set; }
    }
}
