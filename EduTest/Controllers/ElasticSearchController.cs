﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edu.Entity.MySqlEntity;
using Edu.Service;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace EduTest.Controllers
{
    public class ElasticSearchController : Controller
    {
        private readonly ElasticClient _client;

        public ElasticSearchController(IEsClientProvider clientProvider)
        {
            _client = clientProvider.GetClient();
        }

        [HttpPost]
        [Route("value/index")]
        public IIndexResponse Index([FromBody]Log log)
        {
            return _client.IndexDocument(log);
        }

        [HttpPost]
        [Route("value/search")]
        public IReadOnlyCollection<Log> Search([FromQuery]string level)
        {
            return _client.Search<Log>(s => s
                .From(0)
                .Size(10)
                .Query(q => q.Match(m => m.Field(f => f.MachineName).Query(level)))).Documents;
        }
    }
}