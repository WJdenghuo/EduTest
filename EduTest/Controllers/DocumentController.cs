using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EduTest.Controllers
{
    public class DocumentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DotNetCore()
        {
            return View();
        }
        public IActionResult Janus()
        {
            return View();
        }
    }
}