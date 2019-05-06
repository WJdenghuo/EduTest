using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Service
{
    public interface IEsClientProvider
    {
        ElasticClient GetClient();
    }
}
