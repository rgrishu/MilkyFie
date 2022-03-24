using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Interfaces
{
    public interface IUser : IRepository<ApplicationUser>
    {
        Task<Dashboard> GetUserDashBoard(Dashboard entity = null);
        Task<ApplicationUser> GetUserInfo(ApplicationUser entity = null);
        Task<decimal> UserBalanceForAPi(int UserID);
        Task<Response> UpdateUserInfo(ApplicationUser entity);

        Task<Response> FosMapping(FOSMap entity);
        Task<Response> DeleteFosMapping(FOSMap entity);
        Task<IEnumerable<FOSMap>> GetMapedFos(FOSMap entity = null);
        Task<DashboardApi> GetUserDashBoardApi(Dashboard entity = null);
        Task<IEnumerable<UserInfoApi>> GetFosUsers(int UserID);
        Task<Response> FOSBalanceCollection(int UserID, int FosID, decimal Amount);
        Task<Response> UpdateUserDetail(ApplicationUser entity);
        Task<JDataTable<ApplicationUser>> UserFilter(jsonAOData filter = null);
    }
}
