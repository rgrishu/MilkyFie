using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Interfaces
{
    public interface IOrder : IRepository<OrderSchedule>
    {
        Task<IEnumerable<OrderSummary>> GetAllAsync(OrderSummary entity = null);
        Task<IEnumerable<OrderDetail>> GetAllAsync(OrderDetail entity = null);
        Task<Response> ChangeStatus(StatusChangeReq screq);
        Task<Response> UodateOrderDetailStatus(StatusChangeReq screq,int LoginID);
        Task<Response> ActiveDeactiveOrderSchedule(int id, bool Status);
        Task<IEnumerable<ApiOrderSchedule>> GetAllAsyncAPi(OrderSchedule entity = null);
        Task<IEnumerable<ApiOrderSummary>> GetAllAsyncOrderSummaryAPi(OrderSummary entity = null);
        Task<IEnumerable<APIOrderDetail>> GetAllAsyncOrderDetailAPi(OrderDetail entity = null);
        Task<JDataTable<OrderSummary>> OrderSummaryFilter(jsonAOData filter = null);
        Task<JDataTable<OrderDetail>> OrderDetailFilter(jsonAOData filter = null);
        Task<JDataTable<OrderSchedule>> GetScheduleOrdersFilter(jsonAOData filter = null);
    }
}
