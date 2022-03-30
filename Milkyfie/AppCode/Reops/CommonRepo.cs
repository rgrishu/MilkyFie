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
    public class CommonRepo : ICommon
    {
        private IDapperRepository _dapper;
        public CommonRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }
        public async Task<Response> InsertLog(CallBackLog req)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Request", req.Request);
            dbparams.Add("Response", req.Response);
            dbparams.Add("Rdata", req.Rdata);
            dbparams.Add("Apiname", req.Apiname);
            Response inires = await _dapper.InsertAsync<Response>("proc_insertapilog", dbparams, commandType: CommandType.StoredProcedure);
            return inires;
        }

    }
}
