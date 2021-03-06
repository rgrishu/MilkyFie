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
    public class CategoryRepo : IRepository<Category>
    {
        private IDapperRepository _dapper;
        public CategoryRepo(IDapperRepository dapper) => _dapper = dapper;
        public async Task<Response> AddAsync(Category entity)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("CategoryID", entity.CategoryID);
            dbparams.Add("CategoryName", entity.CategoryName);
            dbparams.Add("ParentID", entity.Parent!=null?entity.Parent.ParentID:0);
            dbparams.Add("Icon", entity.Icon);
            dbparams.Add("QueryType", entity.CategoryID == 0 ? "I" : "U");
            var res = await _dapper.InsertAsync<Response>("proc_Category", dbparams, commandType: CommandType.StoredProcedure);
            return res ?? new Response { StatusCode = ResponseStatus.Failed, ResponseText = ResponseStatus.Failed.ToString() };
            // throw new System.NotImplementedException();
        }
        public async Task<Response> DeleteAsync(int id)
        {
            Response res = new Response { StatusCode = ResponseStatus.Failed };
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("CategoryID", id);
                dbparams.Add("CategoryName", "");
                dbparams.Add("ParentID", 0);
                dbparams.Add("Icon", "");
                dbparams.Add("QueryType", "D");
                res = await _dapper.GetAsync<Response>("proc_Category", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }
            return res ?? new Response { StatusCode = ResponseStatus.Failed, ResponseText = ResponseStatus.Failed.ToString() };
        }
        public async Task<IEnumerable<Category>> GetAllAsync(Category entity = null)
        {
            string sqlQuery = @"select c.CategoryID,c.CategoryName,c.Icon,c.IsActive,c.ParentID,
                                        p.CategoryName ParentCategory 
                                 from Category c(nolock) Left join Category p(nolock) on c.ParentID = p.CategoryId";
            Category cc = new Category();
            var dbparams = new DynamicParameters();
            var res = await _dapper.GetAllAsync<Category, Parent, Category>(entity ?? new Category(), sqlQuery, (category, parent) =>
               {
                   category.Parent = parent;
                   return category;
               }, splitOn: "ParentID");
            return res;
        }
        public Task<Response<Category>> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task<Category> GetDetails(object id) => throw new NotImplementedException();
        public Task<IReadOnlyList<Category>> GetDropdownAsync(Category entity) => throw new NotImplementedException();

        #region testOnly
        /// <summary>
        /// How to use Split on with Dataset with Dapper
        /// </summary>
        /// <returns></returns>

        public IEnumerable<Category> GetHierarchy()
        {
            try
            {
                using (IDbConnection db = new SqlConnection("server=.; database=milkyfie; Integrated Security=true"))
                {
                    var parent = db.Query<Category, Parent, Category>
                   (
                       @" select 1 [Status],'Success' ResponseText,c.CategoryID,c.CategoryName,c.ParentID,c.Icon,c.IsActive,
p.CategoryName ParentName 
from 
Category c Left join Category p on c.ParentID = p.CategoryId
where c.IsActive=1 order by CategoryName",
                       (category, parent) =>
                       {
                           category.Parent = parent;
                           return category;
                       }, splitOn: "ParentID"
                   );
                    var cate = db.Query<Category, Parent, Category>
                (
                    @" select 1 [Status],'Success' ResponseText,c.CategoryID,c.CategoryName,c.ParentID,c.Icon,c.IsActive,
p.CategoryName ParentName 
from 
Category c Left join Category p on c.ParentID = p.CategoryId
where c.IsActive=1 order by CategoryName",
                    (category, parent) =>
                    {
                        category.Parent = parent;
                        return category;
                    }, splitOn: "CategoryID"
                );

                    var jsonStringp = Newtonsoft.Json.JsonConvert.SerializeObject(parent);
                    var jsonStringc = Newtonsoft.Json.JsonConvert.SerializeObject(cate);
                }
            }
            catch (Exception ex)
            {

            }
            return new List<Category>();

        }
        #endregion
    }
}
