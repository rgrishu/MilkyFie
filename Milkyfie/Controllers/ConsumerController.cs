using AutoMapper;
using GenricFrame.AppCode.DAL;
using GenricFrame.AppCode.Extensions;
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
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GenricFrame.Controllers
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
