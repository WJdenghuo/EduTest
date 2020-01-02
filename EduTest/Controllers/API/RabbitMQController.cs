using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Edu.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EduTest.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly RpcClient _rpcClient;
        public RabbitMQController(ILogger<RabbitMQController> logger, RpcClient rpcClient)
        {
            _logger = logger;
            _rpcClient = rpcClient;
        }

        [HttpGet]
        [Route("RPCTest")]
        public String Test()
        {
            DealCommand dealCommand = new DealCommand()
            {
                All = true
            };
            var body = JsonHelper.Serialize(dealCommand);
            //仅作为测试，视频处理需要时间很长，相关逻辑可以在回调中实现，
            //事件总线研究中，后续实现源和处理的解耦
            var response = _rpcClient.Call(body);
           
            return response;
        }
    }
}