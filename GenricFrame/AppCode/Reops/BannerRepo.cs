using Dapper;
using GenricFrame.AppCode.DAL;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GenricFrame.AppCode.Reops
{
    public class BannersRepo : IRepository<Banners>
    {
        private IDapperRepository _dapper;
        public BannersRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public async Task<Response> AddAsync(Banners entity)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("BannerID", entity.BannerID);
            dbparams.Add("BackLink", entity.BackLink);
            dbparams.Add("Banner", entity.Banner);
            dbparams.Add("IsActive", entity.IsActive);
            dbparams.Add("QueryType", entity.BannerID == 0 ? "I" : "U");
            var res = await _dapper.InsertAsync<Response>("proc_banner", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response res = new Response();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("BannerID", id);
                dbparams.Add("BackLink", "");
                dbparams.Add("Banner","");
                dbparams.Add("IsActive", true);
                dbparams.Add("QueryType", "D");
                res = await _dapper.GetAsync<Response>("proc_banner", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }
            return res;
        }
       
        public async Task<IEnumerable<Banners>> GetAllAsync(Banners entity = null)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("BannerID",0);
            dbparams.Add("BackLink", "");
            dbparams.Add("Banner","");
            dbparams.Add("IsActive", 1);
            dbparams.Add("QueryType","A");
            var res = await _dapper.GetAllAsync<Banners>("proc_banner", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }

        public Task<Response<Banners>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Banners> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Banners>> GetDropdownAsync(Banners entity)
        {
            throw new NotImplementedException();
        }
    }

}
