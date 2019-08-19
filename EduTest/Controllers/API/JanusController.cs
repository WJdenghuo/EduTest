using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EduTest.Controllers.API
{
    /// <summary>
    /// Janus
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JanusController : ControllerBase
    {
        private readonly ILogger _logger;
        public JanusController(ILogger<JanusController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// janus 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public String Test()
        {
            //创建一个ProcessStartInfo对象 使用系统shell 指定命令和参数 设置标准输出
            var psi = new ProcessStartInfo("ffmpeg", "sudo ffmpeg -i /test/audio.opus -i /test/video.webm  -c:v copy -c:a opus -strict experimental /test/test.webm") { RedirectStandardOutput = true };
            //启动
            try
            {
                var proc = Process.Start(psi);
                var stringbuffer = String.Empty;
                if (proc == null)
                {
                    return "Can not exec.";
                }
                else
                {
                    _logger.LogDebug("-------------Start read standard output--------------");
                    //开始读取
                    using (var sr = proc.StandardOutput)
                    {
                        while (!sr.EndOfStream)
                        {
                            _logger.LogDebug(sr.ReadLine());
                            stringbuffer += sr.ReadLine();
                        }

                        if (!proc.HasExited)
                        {
                            proc.Kill();
                        }
                    }
                    _logger.LogDebug("---------------Read end------------------");
                    _logger.LogDebug($"Total execute time :{(proc.ExitTime - proc.StartTime).TotalMilliseconds} ms");
                    _logger.LogDebug($"Exited Code ： {proc.ExitCode}");
                }
                return stringbuffer;
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
            
        }
    }
}