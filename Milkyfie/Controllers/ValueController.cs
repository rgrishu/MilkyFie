using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenricFrame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValueController : Controller
    {
        [HttpGet,Route(nameof(Get))]
        public IEnumerable<string> Get()
        {
            var userName = User.Identity?.Name;
            return new[] { "value1", "value2" };
        }
    }
}
