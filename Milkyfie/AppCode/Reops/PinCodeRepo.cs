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
    public class PinCodeRepo : IRepository<Pincode>
    {
        private IDapperRepository _dapper;
        public PinCodeRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public Task<Response> AddAsync(Pincode entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Pincode>> GetAllAsync(Pincode entity = null)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Pincode", entity.PinCode);
            var res = await _dapper.GetAllAsync<Pincode>("proc_selectPincode", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }


        public Task<Response<Pincode>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Pincode> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Pincode>> GetDropdownAsync(Pincode entity)
        {
            throw new NotImplementedException();
        }
    }

}
