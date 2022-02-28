using Dapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Reops
{
    public class ErrorLogRepo : IRepository<ErrorLog>
    {
        private IDapperRepository _dapper;
        public ErrorLogRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }
        public async Task<Response> AddAsync(ErrorLog entity)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("ErrorMsg", entity.ErrorMsg);
            dbparams.Add("ErrorFrom", entity.ErrorFrom);
            var res = await _dapper.InsertAsync<Response>("insert into ErrorLog(ErrorMsg,ErrorFrom,EntryOn) Values (@ErrorMsg,@ErrorFrom,@EntryOn)", dbparams, commandType: System.Data.CommandType.Text);
            return res;
        }

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ErrorLog>> GetAllAsync(ErrorLog entity = null)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ErrorLog>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorLog> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ErrorLog>> GetDropdownAsync(ErrorLog entity)
        {
            throw new NotImplementedException();
        }
    }
}
