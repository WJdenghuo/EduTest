using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTest.Models
{
    public class RTCUser
    {
        public RTCUser()
        {
            Enumerator = new List<SelectListItem>(); 
        }
        public List<SelectListItem> Enumerator { get; set; }
        public Int32 UserID { get; set; }
        public Int32 UserName { get; set; }
    }
}
