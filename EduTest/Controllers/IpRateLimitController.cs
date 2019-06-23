using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EduTest.Controllers
{
    public class IpRateLimitController : Controller
    {
        private readonly IpRateLimitOptions _options;
        private readonly IIpPolicyStore _ipPolicyStore;

        public IpRateLimitController(IOptions<IpRateLimitOptions> optionsAccessor, IIpPolicyStore ipPolicyStore)
        {
            _options = optionsAccessor.Value;
            _ipPolicyStore = ipPolicyStore;
        }

        [HttpGet]
        public async Task<IpRateLimitPolicies> Get()
        {
            return await _ipPolicyStore.GetAsync(_options.IpPolicyPrefix);
        }

        /// <summary>
        /// 可将规则存储在数据库中配置ip限制规则（动态）
        /// </summary>
        [HttpPost]
        public async void Post()
        {
            var pol = await _ipPolicyStore.GetAsync(_options.IpPolicyPrefix);

            pol.IpRules.Add(new IpRateLimitPolicy
            {
                Ip = "8.8.4.4",
                Rules = new List<RateLimitRule>(new RateLimitRule[] {
                new RateLimitRule {
                    Endpoint = "*:/api/testupdate",
                    Limit = 100,
                    Period = "1d" }
            })
            });

            await _ipPolicyStore.SetAsync(_options.IpPolicyPrefix, pol);
        }
    }
}