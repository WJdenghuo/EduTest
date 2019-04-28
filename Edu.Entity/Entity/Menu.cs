using SqlSugar;

namespace Edu.Entity
{
    /// <summary>
    /// 整体系统菜单及权限表
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 整体系统菜单及权限表
        /// </summary>
        public Menu()
        {
        }

        private System.Int32 _ID;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 ID { get { return this._ID; } set { this._ID = value; } }

        private System.String _FuncID;
        /// <summary>
        /// 功能ID
        /// </summary>
        public System.String FuncID { get { return this._FuncID; } set { this._FuncID = value; } }

        private System.String _ParentID;
        /// <summary>
        /// 父功能ID
        /// </summary>
        public System.String ParentID { get { return this._ParentID; } set { this._ParentID = value; } }

        private System.String _FuncName;
        /// <summary>
        /// 功能名称
        /// </summary>
        public System.String FuncName { get { return this._FuncName; } set { this._FuncName = value; } }

        private System.String _FuncDes;
        /// <summary>
        /// 功能描述
        /// </summary>
        public System.String FuncDes { get { return this._FuncDes; } set { this._FuncDes = value; } }

        private System.String _FuncType;
        /// <summary>
        /// 菜单or模块
        /// </summary>
        public System.String FuncType { get { return this._FuncType; } set { this._FuncType = value; } }

        private System.Int32 _FuncLevel;
        /// <summary>
        /// 功能层级
        /// </summary>
        public System.Int32 FuncLevel { get { return this._FuncLevel; } set { this._FuncLevel = value; } }

        private System.Int32 _OrderNo;
        /// <summary>
        /// 功能序号
        /// </summary>
        public System.Int32 OrderNo { get { return this._OrderNo; } set { this._OrderNo = value; } }

        private System.String _Target;
        /// <summary>
        /// _blank _parent,_search,_self,_top
        /// </summary>
        public System.String Target { get { return this._Target; } set { this._Target = value; } }

        private System.String _TargetUrl;
        /// <summary>
        /// 目标地址
        /// </summary>
        public System.String TargetUrl { get { return this._TargetUrl; } set { this._TargetUrl = value; } }

        private System.String _SysLogo;
        /// <summary>
        /// 系统标识
        /// </summary>
        public System.String SysLogo { get { return this._SysLogo; } set { this._SysLogo = value; } }

        private System.String _RoleIDs;
        /// <summary>
        /// 
        /// </summary>
        public System.String RoleIDs { get { return this._RoleIDs; } set { this._RoleIDs = value; } }

        private System.Int32? _States;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? States { get { return this._States; } set { this._States = value; } }

        private System.DateTime? _StatesDate;
        /// <summary>
        /// 记录改变最后一次状态的时间
        /// </summary>
        public System.DateTime? StatesDate { get { return this._StatesDate; } set { this._StatesDate = value; } }
    }
}