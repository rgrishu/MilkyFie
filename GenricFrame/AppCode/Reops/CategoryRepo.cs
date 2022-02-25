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
    public class CategoryRepo : IRepository<Category>
    {
        private IDapperRepository _dapper;
        public CategoryRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }
        public async Task<Response> AddAsync(Category entity)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("CategoryID", entity.CategoryID);
            dbparams.Add("CategoryName", entity.CategoryName);
            dbparams.Add("ParentID", entity.ParentID);
            dbparams.Add("Icon", entity.Icon);
            dbparams.Add("QueryType", entity.CategoryID == 0 ? "I" : "U");
            var res = await _dapper.InsertAsync<Response>("proc_Category", dbparams, commandType: CommandType.StoredProcedure);
            return res;
            // throw new System.NotImplementedException();
        }




        public async Task<Response> DeleteAsync(int id)
        {
            Response res = new Response();
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
            return res;
            //throw new System.NotImplementedException();
        }

        //public async Task<IEnumerable<Category>> GetAllAsync(Category entity = null)
        //{
        //    string sqlQuery = @" select 1 [Status],'Success' ResponseText,c.CategoryID,c.CategoryName,c.Icon,c.IsActive,c.ParentID,
        //                                p.CategoryName ParentName 
        //                         from Category c Left join Category p on c.ParentID = p.CategoryId
        //                         where c.IsActive=1 order by CategoryName";
        //    Category cc = new Category();
        //    var dbparams = new DynamicParameters();
        //    var res = _dapper.Get<Category, Parent, Category>(sqlQuery, (category, parent) =>
        //              {
        //                  category.Parent = parent;
        //                  return category;
        //              }, splitOn: "ParentID", dbparams, commandType: CommandType.Text
        //          );
        //    return res;
        //}

        public async Task<IEnumerable<Category>> GetAllAsync(Category entity = null)
        {
            string sqlQuery = @"select 1 [Status],'Success' ResponseText,c.CategoryID,c.CategoryName,c.Icon,c.IsActive,c.ParentID,
                                        p.CategoryName ParentName 
                                 from Category c Left join Category p on c.ParentID = p.CategoryId";
            Category cc = new Category();
            var dbparams = new DynamicParameters();
            var res = await _dapper.GetAllAsync<Category, Parent, Category>(entity ?? new Category(), sqlQuery, (category, parent) =>
               {
                   category.Parent = parent;
                   return category;
               }, splitOn: "ParentID");
            return res;
        }

        public Task<Response<Category>> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Category> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Category>> GetDropdownAsync(Category entity)
        {
            throw new System.NotImplementedException();
        }
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


    }

}
