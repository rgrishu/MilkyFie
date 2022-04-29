using Dapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Reops
{
    public class AccountRepo :IAccount
    {
        private IDapperRepository _dapper;
        public AccountRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public Task<Response> AddAsync(Account entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetAllAsync(Account entity = null)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Account>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Account>> GetDropdownAsync(Account entity)
        {
            throw new NotImplementedException();
        }
        public async Task<ApplicationUser> GetUserDetailForForgetPassword(string MobileNo)
        {
            ApplicationUser res = new ApplicationUser();
            try
            {
                var ires = await _dapper.GetAsync<ApplicationUser>("proc_SelectUserDetailByMobileno", new  { MobileNo }, commandType: CommandType.StoredProcedure);
                res = ires;
            }
            catch (Exception ex)
            { }
            return res;
        }
    }

}
