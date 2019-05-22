using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTest.Infrastructure.Exceptions
{
    public class EduException:Exception
    {
        public EduException() { }
        public EduException(String message) : base(message) { }
        public EduException(String message, Exception exception) : base(message, exception) { }
    }
}
