using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Edu.Entity;
using Edu.Models.Models;
using Edu.Service;
using Edu.Tools;
using EduTest.Auth;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EduTest.Controllers.API
{
    /// <summary>
    /// 测试xml注释文件(controller)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class TestController : ControllerBase
    {       
        public TestController(IRepository<UserInfo> repositoryUserInfo)
        {
            _repositoryUserInfo = repositoryUserInfo;
        }
        private readonly IRepository<UserInfo> _repositoryUserInfo;

        /// <summary>
        /// 测试xml注释文件(action)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        [HttpGet]
        [Route("TestAsync1")]
        public async Task<HttpResponseMessage> TestAsync()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                //httpResponseMessage = await ;
                Content = new StringContent("测试！")
            };
            return await Task.FromResult(httpResponseMessage);
        }
        [HttpGet]
        [Route("GetString")]
        public String GetString()
        {
            return "测试2";
        }

        [HttpPost]
        [Route("PostTest")]
        [Authorize]
        public IActionResult PostTest()
        {
            return Content("测试2");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetToken")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            var user = _repositoryUserInfo.Get(x=>x.UserName==userDto.UserName);
            if (user == null) return Unauthorized("用户名错误");
            if (user.Pwd!=MD5Helper.GetMD5String(userDto.Password))
            {
                return Content("密码错误");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("cnki");
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience,"api"),
                    new Claim(JwtClaimTypes.Issuer,"http://localhost:5200"),
                    new Claim(JwtClaimTypes.Id, user.ID.ToString()),
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.PhoneNumber, user.Phone)
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                access_token = tokenString,
                token_type = "Bearer",
                profile = new
                {
                    sid = user.ID,
                    name = user.UserName,
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                }
            });
        }
    }
}