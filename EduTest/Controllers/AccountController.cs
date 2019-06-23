using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Edu.Entity;
using Edu.Entity.MySqlEntity;
using Edu.Models.Models;
using Edu.Service;
using Edu.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using UserInfo = Edu.Entity.UserInfo;

namespace EduTest.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccount _account;
        private readonly BaseEduContext _baseEduContext;
        public AccountController(IAccount account, BaseEduContext baseEduContext)
        {
            _account = account;
            _baseEduContext= baseEduContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region 管理员登录
        // GET: Account
        [AllowAnonymous]
        public IActionResult  Admin_login()
        {
            //FormsAuthentication.SignOut();
            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Admin_login(LoginModel model)
        {

            Result r =await _account.Login(model);
            if (r.R)
            {
                Edu.Entity.UserInfo userInfo = new Edu.Entity.UserInfo();
                userInfo = r.D as Edu.Entity.UserInfo;

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

        #endregion

        #region 用户登录
        // GET: Account
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            //FormsAuthentication.SignOut();
            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {

            Result r = await _account.Login(model);
            if (r.R)
            {
                Edu.Entity.UserInfo userInfo = new Edu.Entity.UserInfo();
                userInfo = r.D as Edu.Entity.UserInfo;

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
        #endregion
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (userDto==null)
            {
                return NoContent();
            }
            if (_baseEduContext.UserInfo.Where(x=>x.UserName== userDto.UserName).Count()>0)
            {
                return NotFound();
            }
            Edu.Entity.MySqlEntity.UserInfo userInfo = new Edu.Entity.MySqlEntity.UserInfo()
            {
                UserName= userDto.UserName,
                Pwd= MD5Helper.GetMD5String(userDto.Password)
            };
            await _baseEduContext.UserInfo.AddAsync(userInfo);
            if (await _baseEduContext.SaveChangesAsync()>0)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Sid, userInfo.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, userInfo.UserName));

                var timespanExpiry = new TimeSpan(0, 0, 30, 0, 0);
                //identity.AddClaim(new Claim(ClaimTypes.Role, userInfo.Photo));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Home");
            }
            return Ok();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}