using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Service
{
    /// <summary>
    /// 重写实现处理授权失败时返回json,避免跳转登录页
    /// </summary>
    //public class ApiAuthorize : AuthorizeAttribute
    //{
    //    protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
    //    {
    //        base.HandleUnauthorizedRequest(filterContext);

    //        var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
    //        //response.StatusCode = HttpStatusCode.Forbidden;
    //        response.StatusCode = HttpStatusCode.OK;
    //        //移动端要求，正确的应该返回合适的HttpResposeMessage
    //        Result<String> result = new Result<string>()
    //        {
    //            R = "0",
    //            Data = null,
    //            M = "未授权，webapi拒绝访问",
    //            Total = 0
    //        };
    //        response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
    //    }
    //}
}
