using AutoMapper;
using GenricFrame.AppCode.DAL;
using GenricFrame.AppCode.Helper;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Reops;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using GenricFrame.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GenricFrame.Controllers
{
    public class MasterController : Controller
    {
        private IDapperRepository _dapper;
        private IRepository<Category> _category;
        private IRepository<Unit> _unit;
        private IRepository<Product> _product;
        private IMapper _mapper;
        public MasterController(IDapperRepository dapper, IRepository<Category> category, IRepository<Unit> unit, IRepository<Product> product, IMapper mapper)
        {
            _dapper = dapper;
            _category = category;
            _unit = unit;
            _product = product;
            _mapper = mapper;

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
        public async Task<IActionResult> SaveNewProduct(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            StringBuilder sb = new StringBuilder("P_");
            sb.Append(DateTime.Now.ToString("yyyymmddMMss"));
            sb.Append(Path.GetExtension(model.file.FileName));
            var fileres = AppUtility.O.UploadFile(new FileUploadModel
            {
                FilePath = FileDirectories.ProductImage,
                file = model.file,
                FileName = sb.ToString()
            });
            model.ProductImage = sb.ToString();
            if (fileres.StatusCode == Status.Success)
            {
                Product product = _mapper.Map<Product>(model);
                var resp = await _product.AddAsync(product);
            }
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
