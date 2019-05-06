using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Text;

namespace Edu.Service
{
    public class EsClientProvider : IEsClientProvider
    {
        private readonly IConfiguration _configuration;
        private ElasticClient _client;
        public EsClientProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ElasticClient GetClient()
        {
            if (_client != null)
                return _client;

            InitClient();
            return _client;
        }

        private void InitClient()
        {
            var node = new Uri(_configuration["EsUrl"]);
            _client = new ElasticClient(new ConnectionSettings(node).DefaultIndex("demo"));
            
            ////Using a connection pool
            //var nodes = new Uri[]
            //{
            //    new Uri("http://myserver1:9200"),
            //    new Uri("http://myserver2:9200"),
            //    new Uri("http://myserver3:9200")
            //};
            //var pool = new StaticConnectionPool(nodes);
            //var settings = new ConnectionSettings(pool);
            //var client = new ElasticClient(settings);
        }
    }
}
