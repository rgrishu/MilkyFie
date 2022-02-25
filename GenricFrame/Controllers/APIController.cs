using GenricFrame.AppCode.CustomAttributes;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GenricFrame.Controllers
{
    [JWTAuthorize]
    [ApiController]
    [Route("api/")]

    public class APIController : Controller
    {
        private IUserService _userService;
        private IHttpContextAccessor _httpContext;
        private LoginResponse loginResponse;
        private readonly AppicationUser _user;
        protected IRepository<Product> _product;
        public APIController(IHttpContextAccessor httpContext, IUserService userService, IRepository<Product> product)
        {
            _userService = userService;
            _httpContext = httpContext;
            _product = product;
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
                StatusCode = Status.Failed,
                ResponseText = Status.Failed.ToString()
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
                    StatusCode = Status.Success,
                    ResponseText = Status.Success.ToString(),
                    Result = resp.ToList()
                };
            }
            return Json(res);
        }

    }
}
