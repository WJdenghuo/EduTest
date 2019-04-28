using SqlSugar;

namespace Edu.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public UserInfo()
        {
        }

        private System.Int32 _ID;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 ID { get { return this._ID; } set { this._ID = value; } }

        private System.String _GUID;
        /// <summary>
        /// 
        /// </summary>
        public System.String GUID { get { return this._GUID; } set { this._GUID = value; } }

        private System.String _UserName;
        /// <summary>
        /// 
        /// </summary>
        public System.String UserName { get { return this._UserName; } set { this._UserName = value; } }

        private System.String _Pwd;
        /// <summary>
        /// 
        /// </summary>
        public System.String Pwd { get { return this._Pwd; } set { this._Pwd = value; } }

        private System.String _TrueName;
        /// <summary>
        /// 
        /// </summary>
        public System.String TrueName { get { return this._TrueName; } set { this._TrueName = value; } }

        private System.String _Company;
        /// <summary>
        /// 
        /// </summary>
        public System.String Company { get { return this._Company; } set { this._Company = value; } }

        private System.String _Email;
        /// <summary>
        /// 
        /// </summary>
        public System.String Email { get { return this._Email; } set { this._Email = value; } }

        private System.String _Phone;
        /// <summary>
        /// 
        /// </summary>
        public System.String Phone { get { return this._Phone; } set { this._Phone = value; } }

        private System.String _QQ;
        /// <summary>
        /// 
        /// </summary>
        public System.String QQ { get { return this._QQ; } set { this._QQ = value; } }

        private System.String _BZ;
        /// <summary>
        /// 
        /// </summary>
        public System.String BZ { get { return this._BZ; } set { this._BZ = value; } }

        private System.String _oID;
        /// <summary>
        /// 
        /// </summary>
        public System.String oID { get { return this._oID; } set { this._oID = value; } }

        private System.Int32 _RoleID;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 RoleID { get { return this._RoleID; } set { this._RoleID = value; } }

        private System.String _Photo;
        /// <summary>
        /// 
        /// </summary>
        public System.String Photo { get { return this._Photo; } set { this._Photo = value; } }

        private System.DateTime? _CreateDate;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? CreateDate { get { return this._CreateDate; } set { this._CreateDate = value; } }

        private System.Int32? _CreateUser;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? CreateUser { get { return this._CreateUser; } set { this._CreateUser = value; } }

        private System.Int32? _States;
        /// <summary>
        /// state状态 分别是 -1：已删除，1：正常使用，0，禁止登录状态
        /// </summary>
        public System.Int32? States { get { return this._States; } set { this._States = value; } }

        private System.DateTime? _StatesDate;
        /// <summary>
        /// 记录改变最后一次状态的时间
        /// </summary>
        public System.DateTime? StatesDate { get { return this._StatesDate; } set { this._StatesDate = value; } }

        private System.String _IPAdress;
        /// <summary>
        /// 注册Ip地址
        /// </summary>
        public System.String IPAdress { get { return this._IPAdress; } set { this._IPAdress = value; } }
    }
}