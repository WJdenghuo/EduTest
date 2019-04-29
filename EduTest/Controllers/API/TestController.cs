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
using Edu.Models.Data;
using Edu.Models.Models;
using Edu.Service;
using Edu.Tools;
using EduTest.Auth;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
        public IActionResult PostTest()
        {
            return Content("测试2");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetToken")]
        public IActionResult Authenticate([FromBody] UserDto userDto)
        {
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var user = _repositoryUserInfo.Get(x=>x.UserName== userDto.UserName);
            if (user == null) return Unauthorized("用户名错误");
            if (user.Pwd!=MD5Helper.GetMD5String(userDto.Password)) return Unauthorized("密码错误");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Consts.Secret);
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddMinutes(20);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience,"api"),
                    new Claim(JwtClaimTypes.Issuer,"https://localhost:44343/"),
                    new Claim(JwtClaimTypes.Id, user.ID.ToString()),
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.PhoneNumber, user.Phone??"18014087280")
                }),
                NotBefore=DateTime.UtcNow,
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

        [AllowAnonymous]
        [HttpGet]
        [Route("GetToken1")]
        public string GetToken(string userName, string password)
        {
            bool success = ((userName == "user") && (password == "111"));
            if (!success)
                return "";

            JWTTokenOptions jwtTokenOptions = new JWTTokenOptions();

            //创建用户身份标识
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Sid, userName),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "user"),
            };

            //创建令牌
            var token = new JwtSecurityToken(
                issuer: jwtTokenOptions.Issuer,
                audience: jwtTokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: jwtTokenOptions.Credentials
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}