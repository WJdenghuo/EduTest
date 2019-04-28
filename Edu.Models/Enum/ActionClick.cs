
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Models.Enum
{
    public enum ActionClick
    {
        [Description("增加")]
        Add = 1,
        [Description("修改")]
        Mody = 2,
        [Description("删除")]
        Delete = 3,
        [Description("查询")]
        Search = 4,
        [Description("浏览")]
        View = 5,
        [Description("登录")]
        Login = 6,
        [Description("注销")]
        LoginOut = 7,
        [Description("发送短信")]
        SendMsg = 8,
        [Description("其他")]
        Other = 9
    }
}
