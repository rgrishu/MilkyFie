using Milkyfie.AppCode.CustomAttributes;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;

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
        protected IUser _users;
        protected IProduct _product;
        protected IRepository<Category> _category;
        protected IRepository<Frequency> _frequency;
        protected IOrder _order;
        protected IRepository<News> _news;
        protected IRepository<Banners> _banners;
        public APIController(IHttpContextAccessor httpContext, IUserService userService, IProduct product, IRepository<Category> category, IRepository<Frequency> frequency,
            IRepository<News> news, IRepository<Banners> banners, IUser users, IOrder order)
        {
            _userService = userService;
            _httpContext = httpContext;
            _product = product;
            _category = category;
            _frequency = frequency;
            _news = news;
            _banners = banners;
            _users = users;
            _order = order;
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

        #region Master

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
        [HttpPost(nameof(frequency))]
        public IActionResult frequency()
        {
            var res = new Response<List<Frequency>>()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };

            var resp = _frequency.GetAllAsync().Result;
            if (resp != null && resp.Count() > 0)
            {
                res = new Response<List<Frequency>>()
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
            var res = new Response<bannerpopup>()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };

            var resp = _banners.GetAllAsync().Result;
            if (resp != null && resp.Count() > 0)
            {
                res = new Response<bannerpopup>()
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = ResponseStatus.Success.ToString(),
                    Result = new bannerpopup
                    {
                        banners = resp.Where(x => !x.IsPopup).ToList(),
                        popup = resp.Where(x => x.IsPopup).FirstOrDefault()
                    }
                };
            }
            return Json(res);
        }
        #endregion

        #region User
        [HttpPost(nameof(UerInfo))]
        public IActionResult UerInfo(int id)
        {
            var res = new Response<ApplicationUser>()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            var entity = new ApplicationUser()
            {
                Id = id,
            };
            var resp = _users.GetUserInfo(entity).Result;
            if (resp != null)
            {
                res = new Response<ApplicationUser>()
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = ResponseStatus.Success.ToString(),
                    Result = resp
                };
            }
            else
            {
                res.ResponseText = "User Details Not Found";
            }
            return Json(res);
        }
        [HttpPost(nameof(UserBalance))]
        public IActionResult UserBalance(int id)
        {
            var res = new Response<decimal>()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };

            var resp = _users.UserBalanceForAPi(id).Result;
            if (resp != null)
            {
                res = new Response<decimal>()
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = ResponseStatus.Success.ToString(),
                    Result = resp
                };
            }
            else
            {
                res.ResponseText = "User Balance Not Found";
            }
            return Json(res);
        }

        [HttpPost(nameof(UpdateProfile))]
        public IActionResult UpdateProfile(ApplicationUser au)
        {
            var res = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            var resp = _users.UpdateUserInfo(au).Result;
            return Json(resp);
        }
        #endregion
        #region Product
        [HttpPost(nameof(Products))]
        public IActionResult Products(int ProductID = 0, int CategoryID = 0, int ParentCatID = 0)
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
            var resp = _product.GetProduct(req).Result;
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
        #endregion
        #region ORder
        [HttpPost(nameof(ScheduleOrder))]
        public IActionResult ScheduleOrder(ApiOrderSchedule os)
        {
            var res = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            var orderschedule = new OrderSchedule()
            {
                User = new ApplicationUser()
                {
                    Id = int.Parse(os.UserID ?? "0")
                },
                LoginID = int.Parse(os.UserID),
                Product = new Product()
                {
                    ProductID = int.Parse(os.ProductID ?? "0")
                },
                Category = new Category()
                {
                    CategoryID = int.Parse(os.CategoryID ?? "0")
                },
                Frequency = new Frequency()
                {
                    FrequencyID = int.Parse(os.FrequencyID ?? "0"),
                },
                Quantity = int.Parse(os.Quantity ?? "0"),
                StartFromDate = os.StartFromDate,
                ScheduleShift = os.ScheduleShift,
                Description = os.Description
            };
            res = _order.AddAsync(orderschedule).Result;
            return Json(res);
        }


        [HttpPost(nameof(ScheduleOrderDetails))]
        public IActionResult ScheduleOrderDetails(ApiOrderReq os)
        {
            var res = new Response<List<ApiOrderSchedule>>()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString()
            };
            var orderschedule = new OrderSchedule()
            {
                User = new ApplicationUser()
                {
                    Id = int.Parse(os.UserID ?? "0")
                },
                Product = new Product()
                {
                    ProductID = int.Parse(os.ProductID ?? "0")
                },
                Category = new Category()
                {
                    CategoryID = int.Parse(os.CategoryID ?? "0")
                }
            };
            var resp = _order.GetAllAsyncAPi(orderschedule).Result;
            if (resp != null && resp.Count() > 0)
            {
                
                res = new Response<List<ApiOrderSchedule>>()
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = ResponseStatus.Success.ToString(),
                   
                Result = resp.ToList()
                };
        }
            return Json(res);
    }


    [HttpPost(nameof(OrderSummary))]
    public IActionResult OrderSummary(int UserID)
    {
        var res = new Response<List<ApiOrderSummary>>()
        {
            StatusCode = ResponseStatus.Failed,
            ResponseText = ResponseStatus.Failed.ToString()
        };
        var entity = new OrderSummary()
        {
            User = new ApplicationUser()
            {
                Id = UserID,
            }
        };
        var resp = _order.GetAllAsyncOrderSummaryAPi(entity).Result;
        if (resp != null && resp.Count() > 0)
        {
            res = new Response<List<ApiOrderSummary>>()
            {
                StatusCode = ResponseStatus.Success,
                ResponseText = ResponseStatus.Success.ToString(),
                Result = resp.ToList()
            };
        }
        return Json(res);
    }
    [HttpPost(nameof(OrderDetails))]
    public IActionResult OrderDetails(int OrderID)
    {
        var res = new Response<List<APIOrderDetail>>()
        {
            StatusCode = ResponseStatus.Failed,
            ResponseText = ResponseStatus.Failed.ToString()
        };
        var entity = new OrderDetail()
        {
            OrderSummary = new OrderSummary()
            {
                OrderID = OrderID,
            },
        };
        var resp = _order.GetAllAsyncOrderDetailAPi(entity).Result;
        if (resp != null && resp.Count() > 0)
        {
         //resp.ToList().ForEach(c => c.StatusValue = Enum.GetName(typeof(Status),int.Parse(c.Status)));
         res = new Response<List<APIOrderDetail>>()
            {
                StatusCode = ResponseStatus.Success,
                ResponseText = ResponseStatus.Success.ToString(),
                Result = resp.ToList()
            };
        }
        return Json(res);
    }
    [HttpPost(nameof(UpdateOrderStatus))]
    public IActionResult UpdateOrderStatus(StatusChangeReq entity, int UserID)
    {
        var res = new Response()
        {
            StatusCode = ResponseStatus.Failed,
            ResponseText = ResponseStatus.Failed.ToString()
        };
        res = _order.UodateOrderDetailStatus(entity, UserID).Result;
        return Json(res);
    }
    #endregion

}
}
