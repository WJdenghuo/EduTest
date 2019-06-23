using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edu.Entity.MySqlEntity;
using Edu.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduTest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "1")]
    public class MeetController : Controller
    {
        private readonly ILogger _logger;
        private readonly BaseEduContext _baseEduContext;
        public MeetController(ILogger<MeetController> logger, BaseEduContext baseEduContext)
        {
            _logger = logger;
            _baseEduContext = baseEduContext;
        }
        // GET: Meet
        public IActionResult Index()
        {
            _logger.LogDebug($"会议测试-{DateTime.Now}");
            List<MeetModel> meetModels = new List<MeetModel>();
            meetModels = _baseEduContext.Meet
                .Select(x => new MeetModel()
                {
                    ID = x.ID,
                    Creater = x.Creater,
                    CreateTime = x.CreateTime,
                    Des = x.Des,
                    Member = x.Member,
                    Photo = x.Photo,
                    Title = x.Title,
                    UserList = null,
                }).ToList();
            meetModels.ForEach(async x => x.UserList = await UserInfoViewsAsync(x.Member));
            return View(meetModels);
        }
        public async Task<List<UserInfoView>> UserInfoViewsAsync(String memerber)
        {
            char[] separator = { ',' };
            if (String.IsNullOrEmpty(memerber))
            {
                return null;
            }
                
            List<int> ids = memerber.Split(separator).Select(x=>Int32.Parse(x)).ToList();
            List<UserInfoView> userInfoViews = new List<UserInfoView>();
            return await _baseEduContext.UserInfo
                .Where(x => ids.Contains(x.Id))
                .Select(x => new UserInfoView()
                {
                    ID=x.Id,
                    UserName=x.UserName,
                }).ToListAsync();
        }
        // GET: Meet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Meet/Create
        public ActionResult Create()
        {
            Meet meet = new Meet();
            return View(meet);
        }

        // POST: Meet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Meet meet)
        {
            try
            {
                // TODO: Add insert logic here
                await _baseEduContext.Meet.AddAsync(meet);
                if (await _baseEduContext.SaveChangesAsync()>0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Meet/Edit/5
        public ActionResult Edit(int id)
        {
            Meet meet = _baseEduContext.Meet.Where(x => x.ID == id).FirstOrDefault();
            return View(meet);
        }

        // POST: Meet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Meet/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _baseEduContext.Meet.FirstAsync(x => x.ID == id);
                _baseEduContext.Meet.Remove(entity);
                if (await _baseEduContext.SaveChangesAsync()>0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                throw;
            }
        }
        public async Task<IActionResult> BatchDelete(String ids)
        {
            try
            {
                if (String.IsNullOrEmpty(ids))
                {
                    return Json(new { r = false, m = "请至少选择一项！" });
                }
                char[] seperator = { ','};
                List<Int32> intIds = ids.Split(seperator).Select(x=>Int32.Parse(x)).ToList();
                var entityList = await _baseEduContext.Meet.Where(x => intIds.Contains(x.ID)).ToListAsync();
                _baseEduContext.Meet.RemoveRange(entityList);
                if (await _baseEduContext.SaveChangesAsync() > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return Json(new { r = false, m = "删除失败！" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Json(new { r = false, m = ex.Message });
                throw;
            }
        }

        // POST: Meet/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public IActionResult SelectUser()
        {
            List<ZTreeNode> zTreeNodes = new List<ZTreeNode>();
            zTreeNodes.Add(new ZTreeNode()
            {
                id = 1,
                name = "测试机构"
            });
            List<UserInfo> userInfos = new List<UserInfo>();
            userInfos = _baseEduContext.UserInfo.ToList();
            userInfos.ForEach(x => zTreeNodes.Add(new ZTreeNode()
            {
                id=x.Id+1,
                pId=1,
                name=x.TrueName
            }));
            return Json(zTreeNodes);
        }
    }
}