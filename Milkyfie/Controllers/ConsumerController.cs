using AutoMapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Extensions;
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
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Milkyfie.Controllers
{
    
    public class ConsumerController : BaseController
    {
        public ConsumerController(IDapperRepository dapper, IRepository<Category> category, IRepository<Unit> unit, IRepository<Product> product, IMapper mapper) : base(dapper, category, unit, product, mapper)
        {

        }
      
        public IActionResult Index()
        {
            return View();
        }
    }
}
