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
    public class OrderRepo : IRepository<OrderSchedule>
    {
        private IDapperRepository _dapper;
        public OrderRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public async Task<Response> AddAsync(OrderSchedule entity)
        {
            var response = new Response()
            {
                StatusCode = Status.Failed,
                ResponseText = Status.Failed.ToString(),
            };
            try
            {

                var dbparams = new DynamicParameters();
                dbparams.Add("ScheduleID", entity.ScheduleID);
                dbparams.Add("UserID", entity.Product!=null?entity.User.Id:0);
                dbparams.Add("LoginID", entity.LoginID);
                dbparams.Add("ProductID", entity.Product != null ? entity.Product.ProductID : 0);
                dbparams.Add("CategoryID", entity.Category != null ? entity.Category.CategoryID : 0);
                dbparams.Add("FrequencyID", entity.Frequency != null ? entity.Frequency.FrequencyID : 0);
                dbparams.Add("OtherFrequency", entity.OtherFrequency);
                dbparams.Add("Quantity", entity.Quantity);
                dbparams.Add("StartFromDate", entity.StartFromDate);
                dbparams.Add("Remark", entity.Remark);
                dbparams.Add("Description", entity.Description);
                response = await _dapper.InsertAsync<Response>("proc_AddOrderSchedule", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderSchedule>> GetAllAsync(OrderSchedule entity = null)
        {
            IEnumerable<OrderSchedule> oderschedule = new List<OrderSchedule>();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("ProductID", entity != null && entity.Product != null ? entity.Product.ProductID : 0);
                //dbparams.Add("ParentCategoryID", entity != null && entity.Category != null && entity.Category.Parent != null ? entity.Category.Parent.ParentID : 0);
                //dbparams.Add("CategoryID", entity != null && entity.Category != null ? entity.Category.CategoryID : 0);
                string sqlQuery = @"proc_GetOrderSchedule";
                Product cc = new Product();
                var res = await _dapper.GetAllAsyncProc <OrderSchedule, ApplicationUser, Product, Frequency,Unit, Category, Parent, OrderSchedule > (entity ?? new OrderSchedule(), sqlQuery, dbparams, (oderschedule, applicationuser, product, frequency, unit, category, parent) =>
                      {
                          oderschedule.User = applicationuser;
                          oderschedule.Product = product;
                          oderschedule.Frequency = frequency;
                          oderschedule.Unit = unit;
                          oderschedule.Category = category;
                          oderschedule.Category.Parent = parent;
                          return oderschedule;
                      }, splitOn: "ScheduleID,UserID,ProductID,FrequencyID,UnitID,CategoryID,ParentID");
                oderschedule = res;
            }
            catch (Exception ex)
            {

            }
            return oderschedule;

        }

        public Task<Response<OrderSchedule>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderSchedule> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<OrderSchedule>> GetDropdownAsync(OrderSchedule entity)
        {
            throw new NotImplementedException();
        }
    }

}
