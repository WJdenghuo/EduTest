using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Models.Models
{
    public class UserInfoView
    {
        public int ID { get; set; }

       
        public string GUID { get; set; }

       
        public string UserName { get; set; }

        public int RoleID { get; set; }

        public string RoleName { get; set; }

       
        public string TrueName { get; set; }

       
        public string Company { get; set; }

       
        public string Email { get; set; }

       
        public string Phone { get; set; }

       
        public string Photo { get; set; }
 
        public int? States { get; set; }

 
    }
}
