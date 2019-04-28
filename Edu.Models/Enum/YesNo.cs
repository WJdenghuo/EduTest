using System.ComponentModel;

namespace Edu.Models.Enum
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum YesNo
    {

        [Description("否")]
        No = 0,
        [Description("是")]
        Yes = 1
    }
    public enum EnableYesNo
    {

        [Description("不启用")]
        No = 0,
        [Description("启用")]
        Yes = 1
    }
    public enum ExistYesNo
    {

        [Description("没有")]
        No = 0,
        [Description("有")]
        Yes = 1
    }
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum YesNoColor
    {

        [Description("<span style='color:red'>否</span>")]
        No = 0,
        [Description("<span style='color:blue'>是</span>")]
        Yes = 1
    }
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum YesNoSpeak
    {

        [Description("<span style='color:red'>是</span>")]
        No = 0,
        [Description("<span style='color:blue'>否</span>")]
        Yes = 1
    }

    public enum SexEnum
    {

        [Description("男")]
        Man = 0,
        [Description("女")]
        Woman = 1
    }
}
