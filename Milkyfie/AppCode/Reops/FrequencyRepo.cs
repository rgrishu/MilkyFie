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
    public class FrequencyRepo : IRepository<Frequency>
    {
        private IDapperRepository _dapper;
        public FrequencyRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public Task<Response> AddAsync(Frequency entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Frequency>> GetAllAsync(Frequency entity = null)
        {
            var dbparams = new DynamicParameters();
            var res = await _dapper.GetAllAsync<Frequency>("proc_SelectFrequency", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }

      

        public Task<Response<Frequency>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Frequency> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Frequency>> GetDropdownAsync(Frequency entity)
        {
            throw new NotImplementedException();
        }
    }

}
