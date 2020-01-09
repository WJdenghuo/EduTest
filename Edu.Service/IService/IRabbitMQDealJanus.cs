using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Service
{
    public interface IRabbitMQDealJanus
    {
        Task<HttpResponseMessage> DownloadAsync(String path);
    }
}
