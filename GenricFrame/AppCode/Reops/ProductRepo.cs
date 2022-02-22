using Dapper;
using GenricFrame.AppCode.DAL;
using GenricFrame.AppCode.Helper;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using GenricFrame.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GenricFrame.AppCode.Reops
{
  

   public class ProductRepo:IRepository<Product>
    {
        private IDapperRepository _dapper;
        public ProductRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public async Task<Response> AddAsync(Product entity)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("CategoryID", entity.CategoryID);
            dbparams.Add("UnitID", entity.UnitID);
            dbparams.Add("ProductName", entity.ProductName);
            dbparams.Add("Quantity", entity.Quantity);
            dbparams.Add("MRP", entity.MRP);
            dbparams.Add("SellingPrice", entity.SellingPrice);
            dbparams.Add("DiscountType", entity.DiscountType);
            dbparams.Add("Discount", entity.Discount);
            dbparams.Add("IsDiscount", entity.IsDiscount);
            dbparams.Add("ProductImage", entity.ProductImage);
            dbparams.Add("ProductIcon", entity.ProductIcon);
            dbparams.Add("Description", entity.Description);
            dbparams.Add("Remark", entity.Remark);
            var res = await _dapper.InsertAsync<Response>("proc_addproduct", dbparams, commandType: CommandType.StoredProcedure);
            return res;

        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response res = new Response();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("ProductID", id);  
                res = await _dapper.GetAsync<Response>("proc_DeleteProduct", dbparams, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }
            return res;
            //throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllAsync(Product entity = null)
        {
            var dbparams = new DynamicParameters();
            var res = await _dapper.GetAllAsync<Product>("proc_SelectProduct", dbparams, commandType: CommandType.StoredProcedure);
            return res;
        }

        public Task<Response<Product>> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }
        public Task<IReadOnlyList<Product>> GetDropdownAsync(Product entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
