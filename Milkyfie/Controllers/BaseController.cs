using AutoMapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Milkyfie.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class BaseController : Controller
    {
        protected IDapperRepository _dapper;
        protected IRepository<Category> _category;
        protected IRepository<Unit> _unit;
        protected IProduct _product;
      
        protected IMapper _mapper;
        public BaseController(IDapperRepository dapper, IRepository<Category> category, IRepository<Unit> unit,
            IProduct product, IMapper mapper)
        {
            _dapper = dapper;
            _category = category;
            _unit = unit;
            _product = product;           
            _mapper = mapper;

        }
    }
}
