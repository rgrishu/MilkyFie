using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using Paymentgateway.Paytm;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Interfaces
{
    public interface IGateWay : IRepository<GateWay>
    {
        Task<PaytmJSRequest> IntiatePGTransactionForApp(InitiatePaymentGatewayRequest req);
        Task<Response> UpdateGateWayTransaction(UpdatePGTransactionRequest req);
        Task<GatewayRequest> SelectPaymentGateWayDetail();
    }
}
