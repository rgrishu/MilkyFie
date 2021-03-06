using AutoMapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Helper;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using Milkyfie.Models.ViewModel;
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
using Milkyfie.AppCode.Extensions;
using Milkyfie.AppCode.CustomAttributes;

namespace Milkyfie.Controllers
{

    public class OrderController : BaseController//Controller
    {
        protected IOrder _orderschedule;
        public OrderController(IDapperRepository dapper, IRepository<Category> category,
            IRepository<Unit> unit, IProduct product, IOrder orderschedule, IMapper mapper) : base(dapper, category, unit, product, mapper)
        {
            _orderschedule = orderschedule;
        }

        #region Orders
        public async Task<IActionResult> OrderSchedule()
        {
            //var resp = await _category.GetAllAsync(null);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OrderSchedule(int id = 0)
        {
            OrderSchedule orderres = new OrderSchedule();
            if (id != 0)
            {
                IEnumerable<OrderSchedule> lpres = await _orderschedule.GetAllAsync(new OrderSchedule { ScheduleID = id });
                if (lpres != null && lpres.Count() > 0)
                {
                    orderres = lpres.FirstOrDefault();
                }
            }
            return PartialView("PartialView/_OrderSchedule", orderres);
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrderSchedule(OrderSchedule model)
        {
            var userId = User.GetLoggedInUserId<int>();
            model.LoginID = userId;
            var resp = await _orderschedule.AddAsync(model);
            return Json(resp);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderSchedule(StatusChangeReq scr, string type)
        {

            var response = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
            };

            if (type == "A")
            {
                scr.Status = Status.Approved;
            }
            else if (type == "R")
            {
                scr.Status = Status.Reject;
            }
            else
            {
                response.ResponseText = "Invalid Status.!";
                return Json(response);
            }
            response = await _orderschedule.ChangeStatus(scr);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetScheduleOrders()
        {
            var resp = await _orderschedule.GetAllAsync(new OrderSchedule { ScheduleID = 0 });
            return PartialView("PartialView/_OrderScheduleList", resp);
        }

        [HttpPost]
        public async Task<IActionResult> GetScheduleOrdersFilter(jsonAOData jsonAOData, LedgerFilters filters)
        {
            jsonAOData.param = filters;
            var res = (JDataTable<OrderSchedule>)_orderschedule.GetScheduleOrdersFilter(jsonAOData).Result;
            res.Data.ToList().ForEach(c => c.StatusValue = Enum.GetName(typeof(Status), c.Status));
            return Json(res);
        }


        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OrderSummaryList(OrderSummary entity = null)
        {
            var res = await _orderschedule.GetAllAsync(entity);
            return PartialView("PartialView/_Orders", res);
        }
        [HttpPost]
        public IActionResult OrderSummaryFilter(jsonAOData jsonAOData, LedgerFilters filters)
        {
            //jsonAOData.param = new { LedgerID = 0, searchText = jsonAOData.search?.value};
            jsonAOData.param = filters;
            var res = (JDataTable<OrderSummary>)_orderschedule.OrderSummaryFilter(jsonAOData).Result;
            res.Data.ToList().ForEach(c => c.StatusValue = Enum.GetName(typeof(Status), c.Status));
            return Json(res);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OrderSummaryDetailList(OrderDetail entity)
        {
            var res = await _orderschedule.GetAllAsync(entity);
            return PartialView("PartialView/_OrdersSummaryDetails", res);
        }
        [HttpPost]
        public async Task<IActionResult> OrderDetailList(OrderDetail entity)
        {
            var res = await _orderschedule.GetAllAsync(entity);
            return PartialView("PartialView/_OrdersDetails", res);
        }

        [HttpPost]
        public IActionResult OrderDetailListFilter(jsonAOData jsonAOData, LedgerFilters filters)
        {
            //jsonAOData.param = new { LedgerID = 0, searchText = jsonAOData.search?.value};
            jsonAOData.param = filters;
            var res = (JDataTable<OrderDetail>)_orderschedule.OrderDetailFilter(jsonAOData).Result;
            res.Data.ToList().ForEach(c => c.StatusValue = Enum.GetName(typeof(Status),c.Status));
            return Json(res);
        } 

        [HttpPost]
        public async Task<IActionResult> UpdateOrderDetailStatus(StatusChangeReq entity, string type)
        {
            var response = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
            };
            if (type == "D")
            {
                entity.Status = Status.Delivered;
            }
            else if (type == "C")
            {
                entity.Status = Status.Canceled;
            }
            else
            {
                response.ResponseText = "Invalid Status.!";
                return Json(response);
            }
            var userId = User.GetLoggedInUserId<int>();
            response = await _orderschedule.UodateOrderDetailStatus(entity, userId);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> ActDeaOrderSch(int id, bool status)
        {
            var response = new Response()
            {
                StatusCode = ResponseStatus.Failed,
                ResponseText = ResponseStatus.Failed.ToString(),
            };
            response = await _orderschedule.ActiveDeactiveOrderSchedule(id, status);
            return Json(response);
        }

        #endregion

    }
}
