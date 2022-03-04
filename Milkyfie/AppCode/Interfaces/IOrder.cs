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
    }
}
