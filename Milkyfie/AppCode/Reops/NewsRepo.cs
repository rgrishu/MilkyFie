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
    public class NewsRepo : IRepository<News>
    {
        private IDapperRepository _dapper;
        public NewsRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

      

        public async Task<Response> AddAsync(News entity)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("NewsID", entity.NewsID);
            dbparams.Add("NewsTitle", entity.NewsTitle);
            dbparams.Add("NewsDescription", entity.NewsDescription);
            dbparams.Add("IsActive", true);
            dbparams.Add("IsAutoExpired", false);
            dbparams.Add("QueryType", "I");
            var res = await _dapper.InsertAsync<Response>("proc_news", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response res = new Response();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("NewsID", id);
                dbparams.Add("NewsTitle", "");
                dbparams.Add("NewsDescription", "");
                dbparams.Add("IsActive",true);
                dbparams.Add("IsAutoExpired",false);
                dbparams.Add("QueryType", "D");
                res = await _dapper.GetAsync<Response>("proc_news", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }
            return res;
            //throw new System.NotImplementedException();
        }

       
        public async Task<IEnumerable<News>> GetAllAsync(News entity = null)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("NewsID", 0);
            dbparams.Add("NewsTitle", "");
            dbparams.Add("NewsDescription", "");
            dbparams.Add("IsActive", true);
            dbparams.Add("IsAutoExpired", false);
            dbparams.Add("QueryType", "A");
            var res = await _dapper.GetAllAsync<News>("proc_news", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }
        public Task<Response<News>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<News> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<News>> GetDropdownAsync(News entity)
        {
            throw new NotImplementedException();
        }
    }

}
