using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Service.IService
{
    public interface IRabbitMQ
    {
        Task<HttpResponseMessage> DownloadAsync(String path);
    }
}
