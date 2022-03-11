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
    }
}
