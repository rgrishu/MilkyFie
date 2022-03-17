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
}
