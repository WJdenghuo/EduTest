using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edu.Entity.MySqlEntity;
using Edu.Service;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace EduTest.Controllers
{
    [Route("[controller]/[action]")]
    public class ElasticSearchController : Controller
    {
        private readonly ElasticClient _client;

        public ElasticSearchController(IEsClientProvider clientProvider)
        {
            _client = clientProvider.GetClient();
        }

        [HttpPost]
        public IndexResponse Index([FromBody]Log log)
        {
            return _client.IndexDocument(log);
        }

        [HttpPost]
        public IReadOnlyCollection<Log> Search([FromQuery]string level)
        {
            return _client.Search<Log>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.
                            MachineName).Query(level)
                        )
                        && q
                    .Match(m => m
                        .Field(f => f.
                            Level).Query("error")
                        )
                        &&+q
                    .Match(m => m
                        .Field(f => f.
                            Id).Query("6")
                        )
                    )
                ).Documents;
        }
        [HttpPost]
        public IReadOnlyCollection<Log> MachineName([FromQuery]string name)
        {
            return _client.Search<Log>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.
                            MachineName).Query(name)
                        )
                        && q
                    .Match(m => m
                        .Field(f => f.
                            Level).Query("error")
                        )
                        && +q
                    .Match(m => m
                        .Field(f => f.
                            Id).Query("6")
                        )
                    )
                ).Documents;
        }
    }
}