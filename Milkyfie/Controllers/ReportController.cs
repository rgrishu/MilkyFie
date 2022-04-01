using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Helper;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System.Collections.Generic;
using System.Linq;

namespace Milkyfie.Controllers
{
    public class ReportController : BaseController
    {
        protected IReport _report;
        public ReportController(IDapperRepository dapper, IRepository<Category> category,
            IRepository<Unit> unit, IProduct product, IReport report, IMapper mapper) : base(dapper, category, unit, product, mapper)
        {
            _report = report;
        }
        public IActionResult Ledger()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetLedger(Ledger entity)
        {
            // var res = _report.GetAllLedger(entity).Result;
            List<Ledger> ledger = new List<Ledger>();
            var res = (JDataTable<Ledger>)_report.Ledger().Result;
            ledger = res.Data;
            return PartialView("PartialView/_Ledger", ledger);
        }

        [HttpPost]
        public IActionResult LedgerFilter(jsonAOData jsonAOData, LedgerFilters filters)
        {
            //jsonAOData.param = new { LedgerID = 0, searchText = jsonAOData.search?.value};
            jsonAOData.param = filters;
            var res = (JDataTable<Ledger>)_report.Ledger(jsonAOData).Result;
            return Json(res);
        }

        public IActionResult FOSCollectionFilter()
        { 
            return View();
        }
        [HttpPost]
        public IActionResult FOSCollection(jsonAOData jsonAOData, FOSFilters filters)
        {
            jsonAOData.param = filters;
            var res = (JDataTable<FOSCollecionFilter>)_report(jsonAOData).Result;
            return Json(res);
        }
    }

}
