using Dapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using Milkyfie.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Reops
{
    public class UsersRepo : IRepository<ApplicationUser>
    {
        private IDapperRepository _dapper;
        public UsersRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public Task<Response> AddAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<ApplicationUser>> GetAllAsync(ApplicationUser entity = null)
        {
            List<ApplicationUser> res = new List<ApplicationUser>();
             
            try
            {
                var dbparams = new DynamicParameters();

               var ires = await _dapper.GetAllAsync<ApplicationUser>("proc_users", dbparams, commandType: CommandType.StoredProcedure);
                res = ires.ToList();
            }
            catch (Exception ex)
            { }
            return res;
        }


        public Task<Response<ApplicationUser>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetDropdownAsync(ApplicationUser entity = null)
        {
            throw new NotImplementedException();
        }
    }

}
