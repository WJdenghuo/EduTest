using SqlSugar;

namespace Edu.Entity
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class LogInfo
    {
        /// <summary>
        /// 系统操作日志
        /// </summary>
        public LogInfo()
        {
        }

        private System.Int32 _ID;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 ID { get { return this._ID; } set { this._ID = value; } }

        private System.String _Name;
        /// <summary>
        /// 描述
        /// </summary>
        public System.String Name { get { return this._Name; } set { this._Name = value; } }

        private System.String _TableName;
        /// <summary>
        /// 对应表格
        /// </summary>
        public System.String TableName { get { return this._TableName; } set { this._TableName = value; } }

        private System.Int32 _OpType;
        /// <summary>
        /// 日志类型（枚举：1：增加，2：修改，3：删除，4，查询，5：浏览，6：登录，7：注销，9，其他）
        /// </summary>
        public System.Int32 OpType { get { return this._OpType; } set { this._OpType = value; } }

        private System.String _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        public System.String Remark { get { return this._Remark; } set { this._Remark = value; } }

        private System.String _Url;
        /// <summary>
        /// 当前URL
        /// </summary>
        public System.String Url { get { return this._Url; } set { this._Url = value; } }

        private System.DateTime? _CreateDate;
        /// <summary>
        /// 记录时间
        /// </summary>
        public System.DateTime? CreateDate { get { return this._CreateDate; } set { this._CreateDate = value; } }

        private System.Int32? _UserID;
        /// <summary>
        /// 用户名
        /// </summary>
        public System.Int32? UserID { get { return this._UserID; } set { this._UserID = value; } }

        private System.String _IP;
        /// <summary>
        /// 用户访问的IP
        /// </summary>
        public System.String IP { get { return this._IP; } set { this._IP = value; } }
    }
}