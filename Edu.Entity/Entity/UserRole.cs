using SqlSugar;

namespace Edu.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// 
        /// </summary>
        public UserRole()
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
        /// 
        /// </summary>
        public System.String Name { get { return this._Name; } set { this._Name = value; } }

        private System.String _Code;
        /// <summary>
        /// 
        /// </summary>
        public System.String Code { get { return this._Code; } set { this._Code = value; } }

        private System.String _IsAdmin;
        /// <summary>
        /// 
        /// </summary>
        public System.String IsAdmin { get { return this._IsAdmin; } set { this._IsAdmin = value; } }

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