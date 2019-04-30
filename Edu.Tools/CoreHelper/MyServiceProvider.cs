using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Tools
{

    public static class MyServiceProvider
    {
        public static IServiceProvider ServiceProvider
        {
            get; set;
        }
    }
    public static class MyHttpContextClass
    {
        public static IServiceProvider ServiceProvider
        {
            get; set;
        }
    }
}
