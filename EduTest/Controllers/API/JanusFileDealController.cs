﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Edu.Service;
using Edu.Tools;
using EduTest.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EduTest.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class JanusFileDealController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly RpcClient _rpcClient;
        private readonly IRabbitMQDealJanus _rabbitMQDealJanus;
        public JanusFileDealController(ILogger<JanusFileDealController> logger, RpcClient rpcClient, IRabbitMQDealJanus rabbitMQDealJanus)
        {
            _logger = logger;
            _rpcClient = rpcClient;
            _rabbitMQDealJanus = rabbitMQDealJanus;
        }

        [HttpPost]
        [Route("RPCTest")]
        public String Test([FromForm][FromBody]DealCommand dealCommand)
        {
            if (dealCommand==null)
            {
                return "不能为空！";
            }
            var body = JsonHelper.Serialize(dealCommand);
            //仅作为测试，视频处理需要时间很长，相关逻辑可以在回调中实现，
            //事件总线研究中，后续实现源和处理的解耦
            var response = _rpcClient.Call(body);
           
            return response;
        }
        /// <summary>
        /// webm文件获取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件流</returns>
        [HttpGet]
        [Route("Download")]
        public async Task<HttpResponseMessage> Download(String path)
        {
            return await _rabbitMQDealJanus.DownloadAsync(path);
        }
    }
}