using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edu.Entity;
using Edu.Service;
using Microsoft.AspNetCore.Mvc;

namespace EduTest.ViewComponents
{
    public class LeftViewComponent : ViewComponent
    {       
        public LeftViewComponent(IRepository<Menu> repositoryMenu, IRepository<UserInfo> repositoryUserInfo)
        {
            _menuRepositoryMenu = repositoryMenu;
            _menuRepositoryUserInfo = repositoryUserInfo;
        }
        private readonly IRepository<Menu> _menuRepositoryMenu;
        private readonly IRepository<UserInfo> _menuRepositoryUserInfo;

        public IViewComponentResult Invoke()
        {
            UserInfo userInfo = new UserInfo();
            userInfo = _menuRepositoryUserInfo.Get(x => x.UserName == User.Identity.Name);
            if (userInfo.ID == 0)
            {
                return View( null);
            }
            string rid = userInfo.RoleID.ToString();
            var query = _menuRepositoryMenu.GetList(p => ("," + p.RoleIDs).Contains("," + rid + ",") && p.FuncLevel == 2).OrderBy(p => p.OrderNo).GroupBy(p => p.ParentID);
            return View(query);
        }
    }
}