using System;
using System.Collections.Generic;

namespace Edu.Entity.MySqlEntity
{
    public partial class LogInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        public int OpType { get; set; }
        public string Remark { get; set; }
        public string Url { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UserId { get; set; }
        public string Ip { get; set; }
    }
}
