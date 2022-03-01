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

namespace Milkyfie.Controllers
{

    public class OrderController : BaseController//Controller
    {
        protected IRepository<OrderSchedule> _orderschedule;
        public OrderController(IDapperRepository dapper, IRepository<Category> category,
            IRepository<Unit> unit, IRepository<Product> product, IRepository<OrderSchedule> orderschedule, IMapper mapper) : base(dapper, category, unit, product, mapper)
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
        public async Task<IActionResult> SaveOrderSchedule(OrderSchedule model)
        {
            var userId = User.GetLoggedInUserId<int>();
            model.LoginID = userId;
            var resp = await _orderschedule.AddAsync(model);
            return Json(resp);
        }
        [HttpPost]

        [HttpPost]
        public async Task<IActionResult> GetScheduleOrders()
        {
            var resp = await _orderschedule.GetAllAsync(new OrderSchedule { ScheduleID = 0 });
            return PartialView("PartialView/_OrderScheduleList", resp);
        }
        #endregion

    }
}
