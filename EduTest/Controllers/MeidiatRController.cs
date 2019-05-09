using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Edu.Service.MediatR;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduTest.Controllers
{
    public class MeidiatRController : Controller
    {
        public MeidiatRController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;
        public async Task Index()
        {
            //发布请求
            var response = await _mediator.Send(new Ping());
            Debug.WriteLine(response); // "Pong"

            //发布消息
            await _mediator.Publish(new Ping2());
        }
    }
}