using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Edu.Models.Models;
using Edu.Service;
using Edu.Tools;

namespace Edu.Entity
{
    public class Account : IAccount
    {
        private readonly IRepository<UserInfo> _userInfoRepository;
        public Account(IRepository<UserInfo> userInfoRepository) { _userInfoRepository = userInfoRepository; }
        public async Task<Result>  Login(LoginModel model)
        {
            Result result = new Result();
            if (string.IsNullOrEmpty(model.Account))
            {
                result.R = false;
                result.M = "用户名不能为空";
                return result;
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                result.R = false;
                result.M = "密码不能为空";
                return result;
            }

            var loginUser = await Task.FromResult(_userInfoRepository.Get(x => x.UserName == model.Account));
            if (loginUser == null)
            {
                result.R = false;
                result.M = "指定账号的用户不存在";
                return result;
            }

            if (loginUser.States != 1)
            {
                result.R = false;
                result.M = "用户处于不可用状态";
                return result;
            }
            if (loginUser.Pwd != MD5Helper.GetMD5String(model.Password) && loginUser.Pwd != model.Password)
            {
                result.R = false;
                result.M = "登录密码不正确";
                return result;
            }
            
            //认证添加缓存
            result.R = true;
            result.D = loginUser;

            return result; ;
        }
    }
}
