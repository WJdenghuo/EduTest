using System;
using System.Collections.Generic;

namespace Edu.Entity.MySqlEntity
{
    public partial class Menu
    {
        public int Id { get; set; }
        public string FuncId { get; set; }
        public string ParentId { get; set; }
        public string FuncName { get; set; }
        public string FuncDes { get; set; }
        public string FuncType { get; set; }
        public int FuncLevel { get; set; }
        public int OrderNo { get; set; }
        public string Target { get; set; }
        public string TargetUrl { get; set; }
        public string SysLogo { get; set; }
        public string RoleIds { get; set; }
        public int? States { get; set; }
        public DateTime? StatesDate { get; set; }
    }
}
