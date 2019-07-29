using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EduTest.Controllers
{
    public class JanusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Room()
        {
            return View();
        }
        public IActionResult RoomTest()
        {
            return View();
        }
    }
}