using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public CloudMediaController(ILogger<CloudMediaController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 云点播-媒体文件存储测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UploadMediaToCloud() 
        {
            VodUploadClient client = new VodUploadClient("AKIDVaTCKjWC5nVyC95NbZWO0McWBcnumAXv",
                "KSKOD7pIZv5zvXHy4NfiZejllFysjrTs");

            VodUploadRequest request = new VodUploadRequest
            {
                MediaFilePath = "/media/test.mp4",
                //CoverFilePath = "/data/videos/Wildlife.jpg"
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
            return Ok();
        }
        /// <summary>
        /// 接收回调
        /// </summary>
        /// <param name="cloudMediaMessage"></param>
        [HttpPost]
        public void CloudCallBack([FromBody]String cloudMediaMessage)
        {
            _logger.LogInformation($"上传回调消息：{cloudMediaMessage}");
        }

    }
}