using System.ComponentModel;

namespace Edu.Models.Enum
{
    /// <summary>
    /// 数据状态
    /// </summary>
    public enum EnumState
    {
        [Description("已经删除")]
        Delete = -1,
        [Description("禁止使用")]
        Ban = 0,
        [Description("正常使用")]
        Normal = 1
    }
    public enum EnumAudit
    {
        [Description("未通过")]
        NoPass = -1,
        [Description("通过")]
        Pass = 1
    }
    public enum EnumAuditColor
    {
        [Description("<span style='color:red'>未通过</span>")]
        NoPass = -1,
        [Description("<span style='color:blue'>通过</span>")]
        Pass = 1
    }


}
