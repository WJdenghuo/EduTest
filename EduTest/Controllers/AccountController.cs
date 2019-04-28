using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Edu.Entity;
using Edu.Models.Models;
using Edu.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTest.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccount _account;
        public AccountController(IAccount account) { _account = account; }
        public IActionResult Index()
        {
            return View();
        }
        #region 管理员登录
        // GET: Account
        [AllowAnonymous]
        public ActionResult Admin_login()
        {
            //FormsAuthentication.SignOut();
            
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Admin_login(LoginModel model)
        {

            Result r =await _account.Login(model);
            if (r.R)
            {
                UserInfo userInfo = new UserInfo();
                userInfo = r.D as UserInfo;

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Sid, userInfo.ID.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, userInfo.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, userInfo.RoleID.ToString()));
                var timespanExpiry = new TimeSpan(0, 0, 30, 0, 0);
                //identity.AddClaim(new Claim(ClaimTypes.Role, userInfo.Photo));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    var test = User.Identity.IsAuthenticated;
                    //model.ReturnUrl = Url.Action("Index", "Home", new { area = "Admin" });
                    //return RedirectToAction("Index", "Home", new { area = "Admin" });
                    return Json(new { r = true, url = "" });
                }
                return Json(new { r = true, url = model.ReturnUrl });
            }
            else
            {
                return Json(new { r = false, m = "登录失败\n" + r.M });

            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}