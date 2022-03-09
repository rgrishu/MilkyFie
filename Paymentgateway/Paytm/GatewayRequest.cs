using System;
using System.Collections.Generic;
using System.Text;

namespace Paymentgateway.Paytm
{
    public class GatewayRequest
    {
        public string MerchantID { get; set; }
        public string MerchantKey { get; set; }
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public int TID { get; set; }
        public string TransactionID { get; set; }
        public string IndustryType { get; set; }
        public int GatwayId { get; set; }
        public string GatewayName { get; set; }
        public string URL { get; set; }
        public string StatusCheckURL { get; set; }
        public string ENVCode { get; set; }
        public string SuccessURL { get; set; }
        public string FailedURL { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string VPA { get; set; }
        public bool IsLive { get; set; }
    }
    public class GatewayRequest<T>: GatewayRequest
    {
        public T Data { get; set; }
    }
}
