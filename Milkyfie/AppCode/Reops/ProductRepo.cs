using Dapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Helper;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using Milkyfie.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Reops
{


    public class ProductRepo : IProduct
    {
        private IDapperRepository _dapper;
        public ProductRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public async Task<Response> AddAsync(Product entity)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("CategoryID", entity.Category != null ? entity.Category.CategoryID : 0);
            dbparams.Add("UnitID", entity.Unit != null ? entity.Unit.UnitID : 0);
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
            dbparams.Add("Weight", entity.Weight);
            dbparams.Add("PackagingDetail", entity.PackagingDetail);
            dbparams.Add("KeyFeatures", entity.KeyFeatures);
            dbparams.Add("Nutrition", entity.Nutrition);
            var res = await _dapper.InsertAsync<Response>("proc_addproduct", dbparams, commandType: CommandType.StoredProcedure);
            return res;

        }

        public async Task<Response> DeleteAsync(int id)
        {
            Response res = new Response();
            res.StatusCode = ResponseStatus.Failed;
            res.ResponseText = ResponseStatus.Failed.ToString();
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
        }

        


        public async Task<IEnumerable<Product>> GetAllAsync(Product entity = null)
        {
            IEnumerable<Product> product = new List<Product>();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("ProductID", entity != null ? entity.ProductID : 0);
                dbparams.Add("ParentCategoryID", entity != null && entity.Category != null && entity.Category.Parent != null ? entity.Category.Parent.ParentID : 0);
                dbparams.Add("CategoryID", entity != null && entity.Category != null ? entity.Category.CategoryID : 0);
                string sqlQuery = @"proc_SelectProduct";
                Product cc = new Product();
                var res = await _dapper.GetAllAsyncProc<Product, Unit, Category, Parent, Product>(entity ?? new Product(), sqlQuery, dbparams, (product, unit, category, parent) =>
                 {
                     product.Unit = unit;
                     product.Category = category;
                     product.Category.Parent = parent;
                     return product;
                 }, splitOn: "ProductID,UnitID,CategoryID,ParentID");
                product = res;
            }
            catch (Exception ex)
            {

            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetProduct(Product entity = null)
        {
            IEnumerable<Product> product = new List<Product>();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("ProductID", entity != null ? entity.ProductID : 0);
                dbparams.Add("ParentCategoryID", entity != null && entity.Category != null && entity.Category.Parent != null ? entity.Category.Parent.ParentID : 0);
                dbparams.Add("CategoryID", entity != null && entity.Category != null ? entity.Category.CategoryID : 0);
                string sqlQuery = @"proc_GetProduct";
                Product cc = new Product();
                var res = await _dapper.GetAllAsyncProc<Product, Category, Parent, Product>(entity ?? new Product(), sqlQuery, dbparams, (product, category, parent) =>
                {
                    product.Category = category;
                    product.Category.Parent = parent;
                    return product;
                }, splitOn: "ProductID,CategoryID,ParentID");
                product = res;
            }
            catch (Exception ex)
            {

            }
            return product;
        }



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


        public Task<Response<Product>> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Product> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetDropdownAsync(Product entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
