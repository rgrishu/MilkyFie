using GenricFrame.AppCode.DAL;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Reops;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models.FormModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GenricFrame.Controllers
{
    public class MasterController : Controller
    {
        private IDapperRepository _dapper;
        private IRepository<Category> _category;
        private IRepository<Unit> _unit;
        private IRepository<Product> _product;
        public MasterController(IDapperRepository dapper, IRepository<Category> category, IRepository<Unit> unit, IRepository<Product> product)
        {
            _dapper = dapper;
            _category = category;
            _unit = unit;
            _product = product;
        }
        #region Category
        public async Task<IActionResult> Category()
        {

            //var resp = await _category.GetAllAsync(null);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Category(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var resp = await _category.AddAsync(model);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetCategory()
        {

            var resp = await _category.GetAllAsync();
            return PartialView("PartialView/_Category", resp);
        }

        [HttpPost]
        public async Task<IActionResult> GetCategoryDrop()
        {

            var resp = await _category.GetAllAsync();
            return Json(resp);
        }


        [HttpPost]
        public async Task<IActionResult> DelCategory(int id)
        {

            var resp = await _category.DeleteAsync(id);
            return Json(resp);
        }

        #endregion

        #region Products
        public async Task<IActionResult> Product()
        {
            //var resp = await _category.GetAllAsync(null);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewProduct()
        {
            Product objpt = new Product(); 
            return PartialView("PartialView/_Product", objpt);
        }
        [HttpPost]
        public async Task<IActionResult> SaveNewProduct(Product model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var resp = await _product.AddAsync(model);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetProduct()
        {
            var resp = await _product.GetAllAsync();
            return PartialView("PartialView/_ProductList", resp);
        }

        #endregion

        #region Unit
        [HttpPost]
        public async Task<IActionResult> Unit()
        {
            var resp = await _unit.GetDropdownAsync(null);
            return Json(resp);
        }
        #endregion

    }
}
