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
    public class UsersRepo : IRepository<AppicationUser>
    {
        private IDapperRepository _dapper;
        public UsersRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public Task<Response> AddAsync(AppicationUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppicationUser>> GetAllAsync(AppicationUser entity = null)
        {
            throw new NotImplementedException();
        }

        public Task<Response<AppicationUser>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AppicationUser> GetDetails(object id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("UserName", id);
            var res = await _dapper.GetAsync<AppicationUser>("proc_users", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }

        public Task<IReadOnlyList<AppicationUser>> GetDropdownAsync(AppicationUser entity)
        {
            throw new NotImplementedException();
        }
    }

}
