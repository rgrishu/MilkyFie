using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System.Collections.Generic;

namespace Milkyfie.Controllers
{
    public class ReportController : BaseController
    {
        protected IReport _report;
        public ReportController(IDapperRepository dapper, IRepository<Category> category,
            IRepository<Unit> unit, IRepository<Product> product, IReport report, IMapper mapper) : base(dapper, category, unit, product, mapper)
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
            var res = (JDataTable<Ledger>)_report.GetMultiSplits().Result;
            ledger = res.Data;
            return PartialView("PartialView/_Ledger", ledger);
        }

        [HttpPost]
        public IActionResult LedgerFilter(jsonAOData jsonAOData)
        {
            var res = (JDataTable<Ledger>)_report.GetMultiSplits().Result;
            res.recordsTotal = 15;
            res.draw = 10;
           // var ledger = res.Data;
            return Json(res);
        }
    }
}
