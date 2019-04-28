using System.ComponentModel;
namespace Edu.Models.Enum
{
    public enum UserInfo_isAdmin
    {

        [Description("管理用户")]
        admin = 1,
        [Description("普通用户")]
        commonUser = 2
    }
    public enum UserInfo_UserState
    {
        [Description("已删除")]
        Delete = -1,
        [Description("被锁定")]
        Lock = 0,
        [Description("正常登陆")]
        Normal = 1
    }
    public enum UserInfo_UserStateColor
    {
        [Description("<span style='color:red'>已删除</span>")]
        Delete = -1,
        [Description("<span style='color:blue'>被锁定</span>")]
        Lock = 0,
        [Description("正常登陆")]
        Normal = 1
    }
    public enum UserInfo_Role
    {
        [Description("超级管理员")]
        admin = 1,
        [Description("普通用户")]
        User = 2,
        [Description("机构管理员")]
        oadmin = 3,
        [Description("政审人员")]
        political = 4
    }

}
