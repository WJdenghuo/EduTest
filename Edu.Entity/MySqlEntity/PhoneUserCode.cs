using System;
using System.Collections.Generic;

namespace Edu.Entity.MySqlEntity
{
    public partial class PhoneUserCode
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
