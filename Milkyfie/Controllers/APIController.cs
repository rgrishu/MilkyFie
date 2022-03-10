using Milkyfie.AppCode.CustomAttributes;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Milkyfie.Controllers
{
    [JWTAuthorize]
    [ApiController]
    [Route("api/")]

    public class APIController : Controller
    {
        private IUserService _userService;
        private IHttpContextAccessor _httpContext;
        private LoginResponse loginResponse;
        private readonly ApplicationUser _user;
        protected IRepository<Product> _product;
        protected IRepository<Category> _category;
        protected IRepository<News> _news;
        protected IRepository<Banners> _banners;
        public APIController(IHttpContextAccessor httpContext, IUserService userService, IRepository<Product> product, IRepository<Category> category, IRepository<News> news, IRepository<Banners> banners)
        {
            _userService = userService;
            _httpContext = httpContext;
            _product = product;
            _category = category;
            _news = news;
            _banners = banners;
            if (_httpContext != null && _httpContext.HttpContext != null)
            {
                loginResponse = (LoginResponse)_httpContext?.HttpContext.Items["User"];
                _user = loginResponse.Result;
            }
        }

      
        [HttpGet(nameof(loginInfo))]
        public IActionResult loginInfo()
        {
            return Ok(loginResponse);
        }

       
        [HttpPost(nameof(ProductDetails))]
        public IActionResult ProductDetails(int ProductID = 0, int CategoryID = 0, int ParentCatID = 0)
        {
            var res = new Response<List<Product>>()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            var req = new Product()
            {
                ProductID = ProductID,
                Category = new Category()
                {
                    CategoryID = CategoryID,
                    Parent = new Parent()
                    {
                        ParentID = ParentCatID
                    }
                },
            };
            var resp = _product.GetAllAsync(req).Result;
            if (resp != null && resp.Count() > 0)
            {
                res = new Response<List<Product>>()
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = ResponseStatus.Success.ToString(),
                    Result = resp.ToList()
                };
            }
            return Json(res);
        }

        [HttpPost(nameof(CategoryDetails))]
        public IActionResult CategoryDetails()
        {
            var res = new Response<List<Category>>()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
           
            var resp = _category.GetAllAsync().Result;
            if (resp != null && resp.Count() > 0)
            {
                res = new Response<List<Category>>()
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = ResponseStatus.Success.ToString(),
                    Result = resp.ToList()
                };
            }
            return Json(res);
        }
        [HttpPost(nameof(news))]
        public IActionResult news()
        {
            var res = new Response<List<News>>()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };

            var resp = _news.GetAllAsync().Result;
            if (resp != null && resp.Count() > 0)
            {
                res = new Response<List<News>>()
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = ResponseStatus.Success.ToString(),
                    Result = resp.ToList()
                };
            }
            return Json(res);
        }
        [HttpPost(nameof(banner))]
        public IActionResult banner()
        {
            var res = new Response<List<Banners>>()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };

            var resp = _banners.GetAllAsync().Result;
            if (resp != null && resp.Count() > 0)
            {
                res = new Response<List<Banners>>()
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = ResponseStatus.Success.ToString(),
                    Result = resp.ToList()
                };
            }
            return Json(res);
        }

    }
}
