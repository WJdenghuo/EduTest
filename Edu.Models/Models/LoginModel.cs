using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Models.Models
{
    public class LoginModel
    {
        public LoginModel()
        {
            IsRememberLogin = true;
        }
        /// <summary>
        /// 获取或设置 登录账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置 是否记住登录
        /// </summary>
        public bool IsRememberLogin { get; set; }

        /// <summary>
        /// 获取或设置 登录成功后返回地址
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
