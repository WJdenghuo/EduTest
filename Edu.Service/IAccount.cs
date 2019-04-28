using Edu.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Service
{
    public interface IAccount
    {
        Task<Result>  Login(LoginModel model);
    }
}
