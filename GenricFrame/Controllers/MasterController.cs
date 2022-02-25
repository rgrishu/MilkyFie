using AutoMapper;
using GenricFrame.AppCode.DAL;
using GenricFrame.AppCode.Helper;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Reops;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using GenricFrame.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenricFrame.Controllers
{

    public class MasterController : BaseController//Controller
    {
        protected IRepository<Banners> _banner;
        protected IRepository<News> _news;
        public MasterController(IDapperRepository dapper, IRepository<Category> category,
            IRepository<Unit> unit, IRepository<Product> product, IRepository<Banners> banner, IRepository<News> news, IMapper mapper) : base(dapper, category, unit, product, mapper)
        {
            _banner = banner;
            _news = news;
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
            var response = new Response()
            {
                StatusCode = Status.Failed,
                ResponseText = "Failed"
            };
            if (!ModelState.IsValid)
            {
                return Json(response);
            }
            var resp = await _category.AddAsync(model);
            if (resp.StatusCode == Status.Success)
            {
                response.StatusCode = Status.Success;
                response.ResponseText = resp.ResponseText;
            }
            else
            {
                response.ResponseText = resp.ResponseText;
            }
            return Json(response);
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
            var response = new Response()
            {
                StatusCode = Status.Failed,
                ResponseText = "Failed"
            };
            var resp = await _category.DeleteAsync(id);
            if (resp.StatusCode == Status.Success)
            {
                response.StatusCode = Status.Success;
                response.ResponseText = resp.ResponseText;
            }
            else
            {
                response.ResponseText = resp.ResponseText;
            }
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
        public async Task<IActionResult> NewProduct(int id = 0)
        {
            Product prres = new Product();
            if (id != 0)
            {
                IEnumerable<Product> lpres = await _product.GetAllAsync(new Product { ProductID = id });
                if (lpres != null && lpres.Count() > 0)
                {
                    prres = lpres.FirstOrDefault();
                }
            }
            return PartialView("PartialView/_Product", prres);
        }
        [HttpPost]
        public async Task<IActionResult> SaveNewProduct(ProductViewModel model)
        {
           
            var retRes = new Response()
            {
                StatusCode = Status.Failed,
                ResponseText = "Failed"
            };
            if (!ModelState.IsValid)
            {
                retRes.ResponseText = "Invalid Input Data.";
                return Json(retRes);
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
                if (resp.ResponseText == "Success")
                {
                    retRes.StatusCode = Status.Success;
                    retRes.ResponseText = "Product Added Successfull.";
                }
            }
            return Json(retRes);
        }
        [HttpPost]
        public async Task<IActionResult> GetProduct()
        {
            var resp = await _product.GetAllAsync(new Product { ProductID = 0 });
            return PartialView("PartialView/_ProductList", resp);
        }



        [HttpPost]
        public async Task<IActionResult> DelProduct(int id)
        {

            var resp = await _product.DeleteAsync(id);
            return Json(resp);
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

        #region Users
        [HttpPost]
        public async Task<IActionResult> UserForm()
        {
            return PartialView("~/Views/Account/PartialView/_Register.cshtml", new RegisterViewModel { IsAdmin = true });
        }



        #endregion

        #region Banner
        public async Task<IActionResult> Banner()
        {

            //var resp = await _category.GetAllAsync(null);
            return View();
        }
        public async Task<IActionResult> UploadBanner(IFormFile file, string backlink)
        {

            var response = new Response()
            {
                StatusCode = Status.Failed,
                ResponseText = "Failed"
            };
            if (file == null)
            {
                response.ResponseText = "Select a file to Upload.";
                return Json(response);
            }
            try
            {
                StringBuilder sb = new StringBuilder("B_");
                sb.Append(DateTime.Now.ToString("yyyymmddMMss"));
                sb.Append(Path.GetExtension(file.FileName));
                var fileres = AppUtility.O.UploadFile(new FileUploadModel
                {
                    FilePath = FileDirectories.BannerImage,
                    file = file,
                    FileName = sb.ToString(),
                    IsThumbnailRequired = true
                });
                if (fileres.StatusCode == Status.Success)
                {
                    var banner = new Banners()
                    {
                        Banner = sb.ToString(),
                        BackLink = backlink,
                        IsActive = true
                    };
                    var resp = await _banner.AddAsync(banner);
                    return Json(resp);
                }
                else
                {
                    return Json(fileres);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetBanner()
        {
            var resp = await _banner.GetAllAsync();
            return PartialView("PartialView/_Banner", resp);
        }

        [HttpPost]
        public async Task<IActionResult> DelBanner(int id)
        {
            var response = new Response()
            {
                StatusCode = Status.Failed,
                ResponseText = "Failed"
            };
            var resp = await _banner.DeleteAsync(id);
            if (resp.StatusCode == Status.Success)
            {
                response.StatusCode = Status.Success;
                response.ResponseText = resp.ResponseText;
            }
            else
            {
                response.ResponseText = resp.ResponseText;
            }


            return Json(resp);
        }
        #endregion


        #region News
        public async Task<IActionResult> News()
        {

            //var resp = await _category.GetAllAsync(null);
            return View();
        }
        public async Task<IActionResult> AddNews(News model)
        {
            var response = new Response()
            {
                StatusCode = Status.Failed,
                ResponseText = "Failed"
            };
            if (!ModelState.IsValid)
            {
                return View();
            }
            var resp = await _news.AddAsync(model);
            if (resp.StatusCode == Status.Success)
            {
                response.StatusCode = Status.Success;
                response.ResponseText = resp.ResponseText;
            }
            else
            {
                response.ResponseText = resp.ResponseText;
            }
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNews()
        {
            var resp = await _news.GetAllAsync();
            return PartialView("PartialView/_News", resp);
        }

        [HttpPost]
        public async Task<IActionResult> DelNews(int id)
        {
            var response = new Response()
            {
                StatusCode = Status.Failed,
                ResponseText = "Failed"
            };
            var resp = await _news.DeleteAsync(id);
            if (resp.StatusCode == Status.Success)
            {
                response.StatusCode = Status.Success;
                response.ResponseText = resp.ResponseText;
            }
            else
            {
                response.ResponseText = resp.ResponseText;
            }


            return Json(resp);
        }
        #endregion

    }
}
