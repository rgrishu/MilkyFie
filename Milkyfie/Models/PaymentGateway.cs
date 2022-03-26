using System.Collections.Generic;

namespace Milkyfie.Models
{
    public class PaymentGateway
    {
    }
    public class PGModelForRedirection
    {
        public int PGType { get; set; }
        public int TID { get; set; }
        public int Statuscode { get; set; }
        public string Msg { get; set; }
        public string URL { get; set; }
        public IDictionary<string, string> KeyVals { get; set; }

        public PaytmJSRequest paytmJSRequest { get; set; }


    }
    public class PaytmJSRequest
    {
        public string OrderID { get; set; }
        public string Amount { get; set; }
        public string TokenType { get; set; }
        public string Token { get; set; }
        public string MID { get; set; }
        public string PayMode { get; set; }
        public string CallbackUrl { get; set; }
    }


}
