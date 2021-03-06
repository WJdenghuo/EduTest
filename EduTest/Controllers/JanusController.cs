﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Edu.Entity.MySqlEntity;
using Edu.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace EduTest.Controllers
{
    public class JanusController : Controller
    {
        private readonly ILogger _logger;
        private readonly BaseEduContext _baseEduContext;
        private readonly ConnectionMultiplexer _redis;
        public JanusController(ILogger<JanusController> logger, BaseEduContext baseEduContext,ConnectionMultiplexer redis)
        {
            _logger = logger;
            _baseEduContext = baseEduContext;
            _redis = redis;
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
            var data = new List<String>();
            data.Add("janus.plugin.videoroom");
            ViewBag.token = GetToken("janus", data);
            return View();
        }
        public IActionResult RoomNoRecorder()
        {
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
       
        public String GetToken(String relam,List<String> data) 
        {
            //参照janus要求实现
            //<timestamp>,janus,<plugin1>[,plugin2...]:<signature>
            var redisHelper = _redis.GetDatabase();
            var token = String.Empty;
            token=redisHelper.StringGet("janusToken");
            if (!String.IsNullOrEmpty(token))
            {
                return token;
            }
            else
            {
                var hmac = Math.Floor((decimal)DateTime.UtcNow.AddDays(1).Ticks / 1000) + "," + relam + "," + String.Join(',', data);
                redisHelper.StringSet("janusToken", hmac, new TimeSpan(1, 0, 0, 0));
                return $"{hmac}:{EncryptUtils.HmacSha1Sign("janus", hmac)}";
            }
            
        }
    }
}