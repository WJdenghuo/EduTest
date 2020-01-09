
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Edu.Service
{
    public class RabbitMQDealJanus : IRabbitMQDealJanus
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RabbitMQDealJanus(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<HttpResponseMessage> DownloadAsync(string path)
        {
            String browser = String.Empty;
            browser = _httpContextAccessor.HttpContext.Request.Headers["HeaderUserAgent"].ToString().ToUpper();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            //路径不存在，抛一个错误
            if (!File.Exists(path))
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            FileStream fileStream = File.OpenRead(path);
            httpResponseMessage.Content = new StreamContent(fileStream);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("video/webm");
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName =
                    browser.Contains("FIREFOX") || browser.Contains("SAFARI")
                        ? Path.GetFileName(path)
                        : HttpUtility.UrlEncode(Path.GetFileName(path))
                //FileName = HttpUtility.UrlEncode(Path.GetFileName(filePath))
            };
            return await Task.FromResult(httpResponseMessage);
        }
    }
}
