using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edu.Entity.MySqlEntity;
using Edu.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTest.Controllers
{
    [Authorize]
    public class DiscussionController : Controller
    {
        private readonly BaseEduContext _baseEduContext;
        public DiscussionController(BaseEduContext baseEduContext)
        {
            _baseEduContext = baseEduContext;
        }
        public IActionResult Index()
        {
            
            List<MeetModel> meetModels = new List<MeetModel>();
            List<Meet> meets = new List<Meet>();
            meets = _baseEduContext.Meet.ToList();
            meetModels = meets.Select(x => new MeetModel()
            {
                ID=x.ID,
                Photo=x.Photo,
                Title=x.Title,
                Member=x.Member,
                MemberName=""
            }).ToList();
            return View(meetModels);
        }
        public IActionResult Detail()
        {
            return View();
        }
    }
}