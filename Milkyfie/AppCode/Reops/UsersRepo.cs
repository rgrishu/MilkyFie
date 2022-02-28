using Dapper;
using GenricFrame.AppCode.DAL;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using GenricFrame.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GenricFrame.AppCode.Reops
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
            throw new NotImplementedException();
        }

        public Task<Response<ApplicationUser>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetDetails(object id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("UserName", id);
            var res = await _dapper.GetAsync<ApplicationUser>("proc_users", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }

        public Task<IReadOnlyList<ApplicationUser>> GetDropdownAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }

}
