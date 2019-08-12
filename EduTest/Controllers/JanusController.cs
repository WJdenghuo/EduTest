using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edu.Entity.MySqlEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EduTest.Controllers
{
    public class JanusController : Controller
    {
        private readonly ILogger _logger;
        private readonly BaseEduContext _baseEduContext;
        public JanusController(ILogger<JanusController> logger, BaseEduContext baseEduContext)
        {
            _logger = logger;
            _baseEduContext = baseEduContext;
        }
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
            //异常筛选器测试
            try
            {

            }
            catch (Exception e) when (LogException(e))
            {

                throw;
            }
            return View();
        }
        [HttpPost]
        public IActionResult RoomExist(Int32 roomID)
        {
            var room = _baseEduContext.Room.SingleOrDefault(x => x.MeetID == roomID || x.Numeric == roomID);
            if (room!=null)
            {
                return Content(room.Numeric.ToString());
            }
            return Content("");     
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoom(Int64 roomID,Int32 meetID)
        {
            Room room = new Room();
            room.CreateDateTime = DateTime.Now;
            room.Numeric = roomID;
            room.MeetID = meetID;
            await _baseEduContext.Room.AddAsync(room);
            if (await _baseEduContext.SaveChangesAsync() > 0)
            {
                return Content("");
            }
            return Content("");
        }
        public IActionResult RoomOnlySubscribeTest()
        {
            return View();
        }
        public IActionResult RoomLiveTest()
        {
            return View();
        }
        public Boolean LogException(Exception e)
        {
            _logger.LogError(e,e.Message,nameof(JanusController));
            return false;
        }
    }
}