using Dapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Reops
{
    public class OrderRepo : IOrder
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
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
            };
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("ScheduleID", entity.ScheduleID);
                dbparams.Add("UserID", entity.User != null ? entity.User.Id : 0);
                dbparams.Add("LoginID", entity.LoginID);
                dbparams.Add("ProductID", entity.Product != null ? entity.Product.ProductID : 0);
                dbparams.Add("CategoryID", entity.Category != null ? entity.Category.CategoryID : 0);
                dbparams.Add("FrequencyID", entity.Frequency != null ? entity.Frequency.FrequencyID : 0);
                dbparams.Add("OtherFrequency", entity.OtherFrequency);
                dbparams.Add("Quantity", entity.Quantity);
                dbparams.Add("StartFromDate", entity.StartFromDate);
                dbparams.Add("ScheduleShift", entity.ScheduleShift);
                dbparams.Add("Description", entity.Description);
                response = await _dapper.InsertAsync<Response>("proc_AddOrderSchedule", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        public async Task<Response> ChangeStatus(StatusChangeReq screq)
        {
            var response = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
            };
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("ScheduleID", screq.ID);
                dbparams.Add("Status", screq.Status);
                dbparams.Add("Remark", screq.Remark);
                response = await _dapper.InsertAsync<Response>("proc_UpdateScheduleOrderStatus", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        public async Task<Response> ActiveDeactiveOrderSchedule(int id, bool Status)
        {
            var response = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
            };
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("ScheduleID", id);
                dbparams.Add("IsActive", Status);
                response = await _dapper.InsertAsync<Response>("proc_ActiveDeactiveOrderSchedule", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
            return response;
        }


        public async Task<Response> UodateOrderDetailStatus(StatusChangeReq screq, int LoginID)
        {
            var response = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
            };
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("OrderDetailID", screq.ID);
                dbparams.Add("Status", screq.Status);
                dbparams.Add("Remark", screq.Remark);
                dbparams.Add("LoginID", LoginID);
                dbparams.Add("Quantity", screq.Quantity);
                response = await _dapper.InsertAsync<Response>("proc_UpdateOrderDetailStatus", dbparams, commandType: CommandType.StoredProcedure);
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
                dbparams.Add("ScheduleID",0);
                dbparams.Add("UserID", entity != null && entity.User != null ? entity.User.Id : 0);
                //dbparams.Add("ParentCategoryID", entity != null && entity.Category != null && entity.Category.Parent != null ? entity.Category.Parent.ParentID : 0);
              dbparams.Add("CategoryID", entity != null && entity.Category != null ? entity.Category.CategoryID : 0);
                string sqlQuery = @"proc_GetOrderSchedule";
                var res = await _dapper.GetAllAsyncProc<OrderSchedule, ApplicationUser, Product, Frequency, Unit,
                    Category, Parent, OrderSchedule>(entity ?? new OrderSchedule(), sqlQuery,
                    dbparams, (oderschedule, applicationuser, product, frequency, unit, category, parent) =>
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


        public async Task<IEnumerable<ApiOrderSchedule>> GetAllAsyncAPi(OrderSchedule entity = null)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("ProductID", entity != null && entity.Product != null ? entity.Product.ProductID : 0);
            dbparams.Add("ScheduleID", 0);
            dbparams.Add("UserID", entity != null && entity.User != null ? entity.User.Id : 0);
            dbparams.Add("CategoryID", entity != null && entity.Category != null ? entity.Category.CategoryID : 0);
            var res = await _dapper.GetAllAsync<ApiOrderSchedule>("proc_GetOrderSchedule", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }

        public async Task<IEnumerable<ApiOrderSummary>> GetAllAsyncOrderSummaryAPi(OrderSummary entity = null)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("UserID", entity != null && entity.User != null ? entity.User.Id : 0);
            var res = await _dapper.GetAllAsync<ApiOrderSummary>("proc_GetOrderSummary", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }

        public async Task<IEnumerable<APIOrderDetail>> GetAllAsyncOrderDetailAPi(OrderDetail entity = null)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("OrderID", entity.OrderSummary != null ? entity.OrderSummary.OrderID : 0);
            dbparams.Add("UserID", entity.OrderSummary != null && entity.OrderSummary.User != null ? entity.OrderSummary.User.Id : 0);
            dbparams.Add("Shift", entity.OrderShift != null && entity.OrderShift != "0" ? entity.OrderShift : String.Empty);
            dbparams.Add("DateRange", entity.OrderSummary != null && entity.OrderSummary.OrderDate != null ? entity.OrderSummary.OrderDate : String.Empty);
            var res = await _dapper.GetAllAsync<APIOrderDetail>("proc_GetOrderDetails", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }



        public async Task<IEnumerable<OrderSummary>> GetAllAsync(OrderSummary entity = null)
        {
            IEnumerable<OrderSummary> oderssummary = new List<OrderSummary>();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("UserID", entity != null && entity.User != null ? entity.User.Id : 0);
                string sqlQuery = @"proc_GetOrderSummary";
                var res = await _dapper.GetAllAsyncProc<OrderSummary, ApplicationUser, OrderSummary>(entity ?? new OrderSummary(), sqlQuery,
                      dbparams, (oderssummary, applicationuser) =>
                      {
                          oderssummary.User = applicationuser;
                          return oderssummary;
                      }, splitOn: "OrderID,UserID");
                oderssummary = res;
            }
            catch (Exception ex)
            {

            }
            return oderssummary;

        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync(OrderDetail entity)
        {
            IEnumerable<OrderDetail> orderdetail = new List<OrderDetail>();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("OrderID", entity.OrderSummary != null ? entity.OrderSummary.OrderID : 0);
                dbparams.Add("UserID", entity.OrderSummary != null && entity.OrderSummary.User != null ? entity.OrderSummary.User.Id : 0);
                dbparams.Add("Shift", entity.OrderShift != null && entity.OrderShift != "0" ? entity.OrderShift : String.Empty);
                dbparams.Add("DateRange", entity.OrderSummary != null && entity.OrderSummary.OrderDate != null ? entity.OrderSummary.OrderDate : String.Empty);
                string sqlQuery = @"proc_GetOrderDetails";

                var res = await _dapper.GetAllAsyncProc<OrderDetail, Product, OrderSummary, ApplicationUser, OrderDetail>(entity ?? new OrderDetail(), sqlQuery,
                      dbparams, (orderdetail, product, ordersummary, applicationuser) =>
                      {
                          orderdetail.Product = product;
                          orderdetail.OrderSummary = ordersummary;
                          orderdetail.OrderSummary.User = applicationuser;
                          return orderdetail;
                      }, splitOn: "OrderDetailID,ProductID,ScheduleID,UserID");
                orderdetail = res;
            }
            catch (Exception ex)
            {

            }
            return orderdetail;
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
