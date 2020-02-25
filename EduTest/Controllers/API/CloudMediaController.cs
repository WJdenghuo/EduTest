using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Edu.Models.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VodSDK;

namespace EduTest.Controllers.API
{
    /// <summary>
    /// 腾讯云测试
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CloudMediaController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CloudMediaController(ILogger<CloudMediaController> logger,
            IWebHostEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
    }
        /// <summary>
        /// 云点播-媒体文件存储测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UploadMediaToCloud() 
        {
            VodUploadClient client = new VodUploadClient("*",
                "*");

            var urlPath = "/media/test.mp4";

            string webRootPath = _hostingEnvironment.WebRootPath.Replace('\\', '/');
            string contentRootPath = _hostingEnvironment.ContentRootPath.Replace('\\', '/');
            String path = webRootPath + urlPath;
            if (System.IO.File.Exists(path))
            {
                VodUploadRequest request = new VodUploadRequest
                {
                    MediaFilePath = path,
                    //CoverFilePath = "/data/videos/Wildlife.jpg"
                    Procedure= "Adaptive stream"
                };
                try
                {
                    VodUploadResponse response = client.Upload("ap-beijing", request);
                    // 打印媒体 FileId
                    Console.WriteLine(response.FileId);
                }
                catch (Exception e)
                {
                    // 业务方进行异常处理
                    Console.WriteLine(e);
                }
            }
            else
            {

            }
           
            return Ok();
        }
        /// <summary>
        /// 接收回调
        /// </summary>
        [HttpPost]
        public void CloudCallBack([FromBody] Procedure procedure)
        {
            var reader = new StreamReader(Request.Body);
            var contentFromBody = reader.ReadToEndAsync();
            _logger.LogInformation($"上传回调消息：{contentFromBody}");
            _logger.LogInformation($"上传回调消息frombody反序列化成功：{procedure.EventType}");
        }

    }
}