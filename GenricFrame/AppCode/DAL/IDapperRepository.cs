using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace GenricFrame.AppCode.DAL
{
    interface IDapperRepository 
    {
        DbConnection GetDbconnection();
        IDbConnection GetMasterConnection();
        Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<dynamic> GetMultipleAsync<T1, T2>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<dynamic> GetMultipleAsync<T1, T2, T3>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}
