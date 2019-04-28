using SqlSugar;

namespace Edu.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class PhoneUserCode
    {
        /// <summary>
        /// 
        /// </summary>
        public PhoneUserCode()
        {
        }

        private System.Int32 _ID;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 ID { get { return this._ID; } set { this._ID = value; } }

        private System.String _Phone;
        /// <summary>
        /// 
        /// </summary>
        public System.String Phone { get { return this._Phone; } set { this._Phone = value; } }

        private System.String _Code;
        /// <summary>
        /// 
        /// </summary>
        public System.String Code { get { return this._Code; } set { this._Code = value; } }

        private System.DateTime? _CreateDate;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? CreateDate { get { return this._CreateDate; } set { this._CreateDate = value; } }
    }
}