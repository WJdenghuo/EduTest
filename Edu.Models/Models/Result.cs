using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Models.Models
{  
    public class Result
    {
        public bool R { get; set; }
        public string M { get; set; }
        public Object D { get; set; }

    }
    public class Result<T>
    {
        public bool R { get; set; }
        public string M { get; set; }
        public T D { get; set; }

    }   
}
