using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Entity.MySqlEntity
{
    public partial class Meet
    {
        public Int32 ID { get; set; }
        public string Title { get; set; }
        public string Des { get; set; }
        public string Photo { get; set; }
        public string Creater { get; set; }
        public DateTime CreateTime { get; set; }
        public string Member { get; set; }
    }
}
