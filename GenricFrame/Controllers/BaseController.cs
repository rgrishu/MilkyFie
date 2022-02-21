using AutoMapper;
using GenricFrame.AppCode.DAL;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Reops.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenricFrame.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected IDapperRepository _dapper;
        protected IRepository<Category> _category;
        protected IRepository<Unit> _unit;
        protected IRepository<Product> _product;
        protected IMapper _mapper;
        public BaseController(IDapperRepository dapper, IRepository<Category> category, IRepository<Unit> unit, IRepository<Product> product, IMapper mapper)
        {
            _dapper = dapper;
            _category = category;
            _unit = unit;
            _product = product;
            _mapper = mapper;

        }
    }
}
