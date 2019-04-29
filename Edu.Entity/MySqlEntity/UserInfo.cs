using System;
using System.Collections.Generic;

namespace Edu.Entity.MySqlEntity
{
    public partial class UserInfo
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string TrueName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Qq { get; set; }
        public string Bz { get; set; }
        public string OId { get; set; }
        public int RoleId { get; set; }
        public string Photo { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateUser { get; set; }
        public int? States { get; set; }
        public DateTime? StatesDate { get; set; }
        public string Ipadress { get; set; }
    }
}
