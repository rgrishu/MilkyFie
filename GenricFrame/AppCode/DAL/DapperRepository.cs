﻿using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GenricFrame.AppCode.DAL
{
    public class DapperRepository : IDapperRepository, IDisposable
    {
        private readonly IConfiguration _config;
        private readonly string Connectionstring = "SqlConnection";
        public DapperRepository(IConfiguration config)
        {
            _config = config;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public async Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
            {
                var result = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                {
                    var result = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }

        public async Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    using (var tran = db.BeginTransaction())
                    {
                        try
                        {
                            var resultAsync = await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran);
                            result = resultAsync.FirstOrDefault();
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }
            }
            return result;
        }

        public async Task<T> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    using (var tran = db.BeginTransaction())
                    {
                        try
                        {
                            var resultAsync = await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran);
                            result = resultAsync.FirstOrDefault();
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }

            }

            return result;
        }

        public async Task<dynamic> GetMultipleAsync<T1, T2>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                {
                    var result = await db.QueryMultipleAsync(sp, parms, commandType: commandType).ConfigureAwait(false);
                    var res = new
                    {
                        Table1 = result.Read<T1>(),
                        Table2 = result.Read<T2>()
                    };
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<dynamic> GetMultipleAsync<T1, T2, T3>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                {
                    var result = await db.QueryMultipleAsync(sp, parms, commandType: commandType).ConfigureAwait(false);
                    var res = new
                    {
                        Table1 = result.Read<T1>(),
                        Table2 = result.Read<T2>(),
                        Table3 = result.Read<T3>(),
                    };
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public DbConnection GetDbconnection() => new SqlConnection(_config.GetConnectionString(Connectionstring));

        public IDbConnection GetMasterConnection() => new SqlConnection(_config.GetConnectionString("MasterConnection"));
    }
}
