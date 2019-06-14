using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTest.Controllers
{
    public class WebRTCController : Controller
    {
        public IActionResult Index()
        {
            RTCUser rTCUser = new RTCUser();
            Dictionary<Int32, String> keyValuePairs = new Dictionary<int, string>();
            keyValuePairs.Add(1,"老纪");
            keyValuePairs.Add(2, "和二");
            keyValuePairs.Add(3, "黄三爷");
            keyValuePairs.Add(4, "小月");
            keyValuePairs.Add(5, "杏儿");
            keyValuePairs.Add(6, "常四");
            keyValuePairs.Add(7, "刘全");
            keyValuePairs.Add(8, "贵喜");
            foreach (var item in keyValuePairs)
            {
                rTCUser.Enumerator.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(item.Value, item.Key.ToString()));
            }
            return View(rTCUser);
        }
        public IActionResult Index2()
        {
            RTCUser rTCUser = new RTCUser();
            Dictionary<Int32, String> keyValuePairs = new Dictionary<int, string>();
            keyValuePairs.Add(1, "老纪");
            keyValuePairs.Add(2, "和二");
            keyValuePairs.Add(3, "黄三爷");
            keyValuePairs.Add(4, "小月");
            keyValuePairs.Add(5, "杏儿");
            keyValuePairs.Add(6, "常四");
            keyValuePairs.Add(7, "刘全");
            keyValuePairs.Add(8, "贵喜");
            foreach (var item in keyValuePairs)
            {
                rTCUser.Enumerator.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(item.Value, item.Key.ToString()));
            }
            return View(rTCUser);
        }
        public IActionResult Init()
        {
            return View();
        }
        public IActionResult ResponseInit()
        {
            return View();
        }
        public IActionResult SimpleWebRTC()
        {
            return View();
        }
    }
}