using Dapper;
using GenricFrame.AppCode.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace GenricFrame.AppCode.DAL
{
    public class DapperRepository : IDapperRepository, IDisposable
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _dbConnection;

        private readonly string Connectionstring = "SqlConnection";
        public DapperRepository(IConfiguration config, string connectionString = "SqlConnection")
        {
            _config = config;
            Connectionstring = connectionString;
        }
        public DapperRepository(string connectionString = "SqlConnection")
        {
            Connectionstring = connectionString;
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
            //using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
            using (IDbConnection db = new SqlConnection(Connectionstring))
            {
                var result = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                //using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                using (IDbConnection db = new SqlConnection(Connectionstring))
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

        public async Task<IEnumerable<T>> GetAllAsync<T>(T entity, string sp)
        {
            try
            {
                var prepared = PrepareParameters(sp, entity.ToDictionary());
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T>(prepared.preparedQuery, prepared.dynamicParameters, commandType: CommandType.Text);
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
            // using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
            using (IDbConnection db = new SqlConnection(Connectionstring))
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
            //using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
            using (IDbConnection db = new SqlConnection(Connectionstring))
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
                // using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                using (IDbConnection db = new SqlConnection(Connectionstring))
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
                //using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                using (IDbConnection db = new SqlConnection(Connectionstring))
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

        public async Task<GridReader> GetMultipleAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                //using (IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryMultipleAsync(sp, parms, commandType: commandType).ConfigureAwait(false);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public DbConnection GetDbconnection() => new SqlConnection(_config.GetConnectionString(Connectionstring));

        public IDbConnection GetMasterConnection() => new SqlConnection(_config.GetConnectionString("MasterConnection"));

        public IEnumerable<TReturn> Get<T1, T2, TReturn>(string sqlQuery, Func<T1, T2, TReturn> p, string splitOn, DynamicParameters parms = null, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = db.Query<T1, T2, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: commandType);
                    return result;
                };
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<TReturn>> GetAllAsync<T1, T2, TReturn>(T1 entity, string sqlQuery, Func<T1, T2, TReturn> p, string splitOn)
        {
            try
            {
                var prepared = PrepareParameters(sqlQuery, entity.ToDictionary());
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T1, T2, TReturn>(prepared.preparedQuery, p, splitOn: splitOn, param: prepared.dynamicParameters, commandType: CommandType.Text);
                    return result;
                };
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IEnumerable<TReturn>> GetAsync<T1, T2, TReturn>(string sqlQuery, Func<T1, T2, TReturn> p, string splitOn, DynamicParameters parms = null, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(Connectionstring))
                {
                    var result = await db.QueryAsync<T1, T2, TReturn>(sqlQuery, p, splitOn: splitOn, param: parms, commandType: commandType);
                    return result;
                };
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public Parameters PrepareParameters(string sqlQuery, Dictionary<string, dynamic> args = null)
        {
            var result = new Parameters();
            try
            {
                string condtion = " Where 1=1 ";
                StringBuilder sb = new StringBuilder(condtion);
                List<arg> argList = new List<arg>();
                var dbParam = new DynamicParameters();
                if (args != null)
                {
                    string val = string.Empty;
                    foreach (var pair in args)
                    {
                        val = Convert.ToString(pair.Value);
                        if (!string.IsNullOrEmpty(val) && !val.Equals("false", StringComparison.OrdinalIgnoreCase) && val != "0")
                        {
                            val = val.Equals("true", StringComparison.OrdinalIgnoreCase) ? "1" : val;
                            dbParam.Add(pair.Key, pair.Value);
                            sb.Append(" and ");
                            sb.Append(pair.Key);
                            sb.Append("=");
                            sb.Append("@");
                            sb.Append(pair.Key);
                            //sb.Append("=");
                            //sb.Append("'");
                            //sb.Append(val);
                            //sb.Append("'");
                            argList.Add(new arg
                            {
                                Key = pair.Key,
                                Value = Convert.ToString(pair.Value)
                            });
                        }
                    }
                }
                string Concat = string.Concat(sqlQuery, sb);
                result = new Parameters
                {
                    dynamicParameters = dbParam,
                    preparedQuery = Concat,
                    arguments = argList
                };
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}
