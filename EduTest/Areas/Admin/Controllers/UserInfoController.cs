using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edu.Entity;
using Edu.Models.Models;
using Edu.Service;
using Edu.Service.Admin;
using Edu.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduTest.Areas.Admin.Controllers
{
    [Authorize(Roles = "1")]
    public class UserInfoController : Controller
    {
        
        public UserInfoController(IRepository<UserInfo> userInfoRepository, IRepository<UserRole> repositoryUserRole)
        {
            _userInfoRepository = userInfoRepository;
            _userRoleRepository = repositoryUserRole;
        }
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        // GET: UserInfo
        public ActionResult Index(int? roleID, int? state, DateTime? startT, DateTime? endT, string sn = "", int pageNo = 1)
        {
            Paging paging = new Paging();
            paging.PageNumber = pageNo;

            var query = _userInfoRepository.GetSugarQueryable(p => p.UserName.Contains(sn) || p.TrueName.Contains(sn) || p.Email.Contains(sn) || p.Phone.Contains(sn));
            query = query.WhereIF(roleID != null, it => it.RoleID == roleID).
                WhereIF(state != null, it => it.States == state).
                WhereIF(startT != null, it => it.CreateDate > startT).
                WhereIF(endT != null, it => it.CreateDate > endT);
            paging.Amount = query.Count();
            paging.EntityList = query.OrderBy(p => p.CreateDate, SqlSugar.OrderByType.Desc).Skip(paging.PageSiz * paging.PageNumber).Take(paging.PageSiz).ToList();
            ViewBag.sn = sn;
            ViewBag.roleID = roleID;
            ViewBag.PageNo = pageNo;//页码
            ViewBag.PageCount = paging.PageCount;//总页数

            var list = from m in paging.EntityList as IEnumerable<UserInfo>
                       join n in _userRoleRepository.ListAll() on m.RoleID equals n.ID into tempa
                       from a in tempa
                       select new UserInfoView
                       {
                           ID = m.ID,
                           GUID = m.GUID,
                           Phone = m.Phone,
                           Photo = m.Photo,
                           Company = m.Company,
                           Email = m.Email,
                           RoleID = m.RoleID,
                           RoleName = a.Name,
                           States = m.States,
                           UserName = m.UserName,
                           TrueName = m.TrueName
                       };

            return View(list);
        }

        // GET: UserInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserInfo/Create
        public ActionResult Create(int? id)
        {
            var old = new UserInfo();
            List<SelectListItem> LsRole = new List<SelectListItem>();
            var userroles = _userRoleRepository.ListAll();
            if (id.HasValue)
            {
                old = _userInfoRepository.GetByID(id.Value);
                LsRole.AddRange(new SelectList(userroles, "ID", "NAME", old.RoleID));
            }
            else
            {
                LsRole.AddRange(new SelectList(userroles, "ID", "NAME"));
            }
            ViewBag.RoleID = LsRole;
            return View(old);
        }

        // POST: UserInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserInfo user)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            bool result = false;
            var old = _userInfoRepository.GetByID(id);
            //new DBLogService().insert(ActionClick.Delete, old);
            result=_userInfoRepository.Delete(old);
            if (result)
            {
                return Json(new { r = true });
            }
            else
            {
                return Json(new { r = false, m = "操作失败" });
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModyPwd(int userid)
        {
            //ViewBag.refUrl = Request.UrlReferrer;//修改后返回上层url
            var user = _userInfoRepository.GetByID(userid);
            return PartialView("_ModyPwd", user);
        }
        [HttpPost]
        public ActionResult ModyPwd(UserInfo user)
        {
            bool result = false;
            var old = _userInfoRepository.GetByID(user.ID);
            old.Pwd = MD5Helper.GetMD5String(user.Pwd);
            result = _userInfoRepository.Update(old);

            //new DBLogService().insert(ActionClick.Other, user, "修改密码");
            if (result)
            {
                return Json(new { r = true });
            }
            else
            {
                return Json(new { r = false, m = "操作失败" });

            }
        }
        public ActionResult LoadSearch(int? roleID, int? state, DateTime? startT, DateTime? endT, string sn = "")
        {
            ViewBag.sn = sn;
            ViewBag.state = state;
            ViewBag.startT = startT;
            ViewBag.endT = endT;
            List<SelectListItem> LsRole = new List<SelectListItem>();
            var userroles = _userInfoRepository.ListAll();
            LsRole.AddRange(new SelectList(userroles, "ID", "NAME", roleID));
            ViewBag.roleID = LsRole;
            return PartialView("_Search");
        }
    }
}