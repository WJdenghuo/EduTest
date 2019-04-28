using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edu.Entity;
using Edu.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTest.Areas.Admin.Controllers
{
    //[Authorize(Roles = "1")]
    [Area("Admin")]
    [AllowAnonymous]
    //[Route("[area]/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IRepository<Menu> _menuRepository;
        public HomeController(IRepository<Menu> repository) { _menuRepository = repository; }

        public IActionResult Index()
        {
            return View();
        }        
        public ActionResult Right()
        {
            return View();
        }
        public ActionResult GetLeft()
        {

            if (User.Claims.SingleOrDefault(s => s.Type == System.Security.Claims.ClaimTypes.Sid).Value == "0")
            {
                return PartialView("_Left", null);
            }
            string rid = User.Claims.SingleOrDefault(s => s.Type == System.Security.Claims.ClaimTypes.Role).Value;
            var query = _menuRepository.GetList(p => ("," + p.RoleIDs).Contains("," + rid + ",") && p.FuncLevel == 2).OrderBy(p => p.OrderNo).GroupBy(p => p.ParentID);
            //var query = unitOfWork.DMenu.GetCache(p => ("," + p.RoleIDs).Contains("," + rid + ",") && p.FuncLevel == 2, true).OrderBy(p => p.OrderNo).GroupBy(p => p.ParentID);
            return PartialView("_Left", query);
        }
    }
}