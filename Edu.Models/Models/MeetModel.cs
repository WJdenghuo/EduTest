using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Models.Models
{
    public class MeetModel
    {
        public MeetModel()
        {
            UserList = new List<UserInfoView>();
        }
        public Int32 ID { get; set; }
        public string Title { get; set; }
        public string Des { get; set; }
        public string Photo { get; set; }
        public string Creater { get; set; }
        public DateTime CreateTime { get; set; }
        public string Member { get; set; }
        public string MemberName { get; set; }
        public List<UserInfoView> UserList { get; set; }
    }
}
