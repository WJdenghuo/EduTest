using System;
using System.Collections.Generic;
using System.Text;

namespace EduTest.Models.Models
{
    public class DealCommand
    {
        public DealCommand() 
        {
            UserIDs = new List<string>();
        }
        public Boolean All { get; set; }
        public String RoomID { get; set; }
        public DateTime CreateDate { get; set; }
        public List<String> UserIDs { get; set; }
    }
}
