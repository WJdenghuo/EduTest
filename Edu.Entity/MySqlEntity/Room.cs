using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Entity.MySqlEntity
{
    public partial class Room
    {
        public Int32 ID { get; set; }
        public Int64 Numeric { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Int32 MeetID { get; set; }
    }
}
