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
    public class UsersRepo : IUser
    {
        private IDapperRepository _dapper;
        public UsersRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }
        public async Task<JDataTable<ApplicationUser>> UserFilter(jsonAOData filter = null)
        {
            JDataTable<ApplicationUser> d = new JDataTable<ApplicationUser>();
            try
            {
                d = await _dapper.GetMultipleAsync<ApplicationUser,Pincode, ApplicationUser>("proc_SelectuserFilter", filter,
                    (applicationuser,pincode) =>
                    {
                        return applicationuser;
                    }, splitOn: "PincodeID");
                d.recordsFiltered = d.PageSetting.TotoalRows;//d.Data.Count();
                                                             // d.recordsFiltered = d.Data.Count();
                d.recordsTotal = d.PageSetting.TotoalRows;
            }
            catch (Exception ex)
            {

            }
            return d;
        }

        public async Task<Response> UpdateUserDetail(ApplicationUser entity)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("ID", entity.Id);
            dbparams.Add("Name", entity.Name);
            dbparams.Add("Address", entity.Address);
            dbparams.Add("PhoneNumber", entity.PhoneNumber);
            dbparams.Add("Email", entity.Email);
            dbparams.Add("Pincode", entity.Pincode);
            var res = await _dapper.InsertAsync<Response>("proc_UpdateUserDetail", dbparams, commandType: CommandType.StoredProcedure);
            return res;
            // throw new System.NotImplementedException();
        }


        public async Task<Response> AddAsync(ApplicationUser entity)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("UserID", entity.Id);
            dbparams.Add("LoginID", entity.UserId);
            dbparams.Add("Amount", entity.Balance);
            var res = await _dapper.InsertAsync<Response>("proc_AddBalance", dbparams, commandType: CommandType.StoredProcedure);
            return res;
            // throw new System.NotImplementedException();
        }


        public async Task<Response> DeleteAsync(int id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Id", id);
            var res = await _dapper.GetAsync<Response>("proc_DeleteUser", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }


        public async Task<IEnumerable<ApplicationUser>> GetAllAsync(ApplicationUser entity = null)
        {
            List<ApplicationUser> res = new List<ApplicationUser>();

            try
            {
                var dbparams = new DynamicParameters();
               
                dbparams.Add("UserID", entity!=null? entity.Id:0);
                var ires = await _dapper.GetAllAsync<ApplicationUser>("proc_users", dbparams, commandType: CommandType.StoredProcedure);
                res = ires.ToList();
            }
            catch (Exception ex)
            { }
            return res;
        }
        public async Task<Dashboard> GetUserDashBoard(Dashboard entity = null)
        {
            Dashboard dashboard = new Dashboard();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("UserID", entity != null && entity.User != null ? entity.User.Id : 0);

                string sqlQuery = @"proc_SelectUserDashBoard";
                var res = await _dapper.GetAllAsyncProc<Dashboard, ApplicationUser, Dashboard>(entity ?? new Dashboard(), sqlQuery,
                    dbparams, (dashboard, applicationuser) =>
                    {
                        dashboard.User = applicationuser;

                        return dashboard;
                    }, splitOn: "UserID");
                dashboard = res.FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return dashboard;

        }

        public async Task<DashboardApi> GetUserDashBoardApi(Dashboard entity = null)
        {
            DashboardApi dashboard = new DashboardApi();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("UserID", entity != null && entity.User != null ? entity.User.Id : 0);
                dashboard = await _dapper.GetAsync<DashboardApi>("proc_SelectUserDashBoard", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
            return dashboard;
        }


        public Task<Response<ApplicationUser>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<ApplicationUser> GetUserInfo(ApplicationUser entity = null)
        {
            ApplicationUser res = new ApplicationUser();

            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("UserName", entity != null ? entity.UserName : String.Empty);
                dbparams.Add("Role", entity != null ? entity.Role : String.Empty);
                dbparams.Add("UserID", entity != null ? entity.Id : 0);
                var ires = await _dapper.GetAllAsync<ApplicationUser>("proc_users", dbparams, commandType: CommandType.StoredProcedure);
                res = ires.FirstOrDefault();
            }
            catch (Exception ex)
            { }
            return res;
        }
        public async Task<Response> UpdateUserInfo(ApplicationUser entity)
        {
            var res = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            if (entity == null)
            {
                res.ResponseText = "Invalid Request Data!";
                return res;
            }
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("UserID", entity.Id);
                dbparams.Add("Name", entity.Name);
                dbparams.Add("Email", entity.Email);
                dbparams.Add("PhoneNumber", entity.PhoneNumber);
                dbparams.Add("Address", entity.Address);
                dbparams.Add("Pincode", entity.Pincode);
                var ires = await _dapper.GetAllAsync<Response>("prco_UserUpdate", dbparams, commandType: CommandType.StoredProcedure);
                res = ires.FirstOrDefault();
            }
            catch (Exception ex)
            { }
            return res;
        }
        public async Task<decimal> UserBalanceForAPi(int UserID)
        {
            decimal balance = 0;
            try
            {
                //var dbparams = new DynamicParameters();
                //dbparams.Add("UserID", UserID);
                string query = "Select Balance from UserBalance where UserID=@UserID";
                balance = await _dapper.GetAsync<decimal>(query, new { UserID }, commandType: CommandType.Text);

            }
            catch (Exception ex)
            { }
            return balance;
        }


        #region FOS

        public async Task<IEnumerable<UserInfoApi>> GetFosUsers(int UserID)
        {
            List<UserInfoApi> res = new List<UserInfoApi>();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("UserID", UserID);
                var ires = await _dapper.GetAllAsync<UserInfoApi>("proc_SelectFosCustomers", dbparams, commandType: CommandType.StoredProcedure);
                res = ires.ToList();
            }
            catch (Exception ex)
            { }
            return res;
        }

        public async Task<Response> FosMapping(FOSMap entity)
        {
            var res = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            if (entity == null)
            {
                res.ResponseText = "Invalid Request Data!";
                return res;
            }
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("FOSMapID", entity.FOSMapID);
                dbparams.Add("FOSID", entity.Users.Id);
                dbparams.Add("PincodeID", entity.pincode.PincodeID);
                dbparams.Add("QueryType", "I");
                var ires = await _dapper.GetAllAsync<Response>("proc_FosMaping", dbparams, commandType: CommandType.StoredProcedure);
                res = ires.FirstOrDefault();
            }
            catch (Exception ex)
            { }
            return res;
        }
        public async Task<Response> DeleteFosMapping(FOSMap entity)
        {
            var res = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            if (entity == null)
            {
                res.ResponseText = "Invalid Request Data!";
                return res;
            }
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("FOSMapID", entity.FOSMapID);
                dbparams.Add("FOSID", 0);
                dbparams.Add("PincodeID", 0);
                dbparams.Add("QueryType", "D");
                var ires = await _dapper.GetAllAsync<Response>("proc_FosMaping", dbparams, commandType: CommandType.StoredProcedure);
                res = ires.FirstOrDefault();
            }
            catch (Exception ex)
            { }
            return res;
        }

        public async Task<IEnumerable<FOSMap>> GetMapedFos(FOSMap entity = null)
        {
            IEnumerable<FOSMap> fosmap = new List<FOSMap>();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("FOSMapID", 0);
                dbparams.Add("FOSID", 0);
                dbparams.Add("PincodeID", 0);
                dbparams.Add("QueryType", "S");
                string sqlQuery = @"proc_FosMaping";
                var res = await _dapper.GetAllAsyncProc<FOSMap, ApplicationUser, Pincode, FOSMap>(entity ?? new FOSMap(), sqlQuery,
                      dbparams, (fosmap, applicationuser, pincode) =>
                      {
                          fosmap.Users = applicationuser;
                          fosmap.pincode = pincode;
                          return fosmap;
                      }, splitOn: "FOSMapID,UserID,PincodeID");
                fosmap = res;
            }
            catch (Exception ex)
            {

            }
            return fosmap;

        }

        public async Task<Response> FOSBalanceCollection(int UserID, int FosID, decimal Amount)
        {
            var res = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("UserID", UserID);
                dbparams.Add("FOSID", FosID);
                dbparams.Add("Amount", Amount);
                res = await _dapper.GetAsync<Response>("proc_FOSBalanceCollection", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            { }
            return res;
        }

        #endregion


        public async Task<IReadOnlyList<ApplicationUser>> GetDropdownAsync(ApplicationUser entity = null)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetDetails(object id)
        {
            throw new NotImplementedException();
        }
    }

}
