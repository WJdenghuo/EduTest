using System;
using System.Collections.Generic;

namespace Edu.Entity.MySqlEntity
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string IsAdmin { get; set; }
        public int? States { get; set; }
        public DateTime? StatesDate { get; set; }
    }
}
