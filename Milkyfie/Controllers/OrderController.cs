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
        #endregion

    }
}
