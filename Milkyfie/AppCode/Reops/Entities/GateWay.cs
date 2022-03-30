using Milkyfie.Models;

namespace Milkyfie.AppCode.Reops.Entities
{
    public class GateWay
    {


    }
    public class InitiatePaymentGatewayRequest 
    {
        public string Amount { get; set; }
        public string UserID { get; set; }
    }
    public class InitiatePaymentGatewayResponse:Response
    {
        public string OrderID { get; set; }
        public string TransactionID { get; set; }
        public string MerchantId { get; set; }
        public string Merchantkey { get; set; }
        public string URL { get; set; }
        public string Amount { get; set; }
        public string CallBackUrl { get; set; }
    }
    public class UpdatePGTransactionRequest
    {
        public string OrderID { get; set; }
        public string VendorID { get; set; }
        public string LiveID { get; set; }
        public string Remark { get; set; }
        public string Amount { get; set; }
        public int Status { get; set; }
    }

}
