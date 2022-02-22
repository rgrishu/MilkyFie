﻿using Microsoft.AspNetCore.Mvc;

namespace GenricFrame.Controllers
{
    public class WebSiteController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        [Route("Gallery")]
        public IActionResult Gallery()
        {
            return View();
        }
        [Route("Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("About")]
        public IActionResult About()
        {
            return View();
        }
        [Route("Products")]
        public IActionResult Products()
        {
            return View();
        }
    }
}
