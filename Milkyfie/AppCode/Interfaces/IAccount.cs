using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Interfaces
{
    public interface IAccount : IRepository<Account>
    {
        Task<ApplicationUser> GetUserDetailForForgetPassword(string MobileNo);
    }
}
