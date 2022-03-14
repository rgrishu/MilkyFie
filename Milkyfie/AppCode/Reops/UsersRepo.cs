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


        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<ApplicationUser>> GetAllAsync(ApplicationUser entity = null)
        {
            List<ApplicationUser> res = new List<ApplicationUser>();

            try
            {
                var dbparams = new DynamicParameters();

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
