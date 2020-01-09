using AspectCore.Extensions.Autofac;
using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Edu.Entity;
using Edu.Entity.MySqlEntity;
using Edu.Models.Data;
using Edu.Models.Models;
using Edu.Service;
using Edu.Service.Admin;
using Edu.Service.MediatR;
using Edu.Tools;
using Edu.Tools.Redis;
using EduTest.Hubs;
using EduTest.Infrastructure.Filters;
using EduTest.Infrastructure.HostedService;
using EduTest.Infrastructure.Middlewares;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EduTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        //private readonly ILogger _logger;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            /*
             * ********************RateLimit***********************
             */
            // needed to load configuration from appsettings.json
            services.AddOptions();

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            services.Configure<RedisSetting>(Configuration);
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.None;
            });
            //HealthyChecks
            //具体信息参照源码 https://github.com/xabaril/AspNetCore.Diagnostics.HealthChecks
            //services.AddHealthChecks()
            //    .AddMySql(Configuration.GetConnectionString("DefaultConnection"))
            //    .AddRedis(Configuration.GetConnectionString("RedisConnection")
            //);
            //services.AddHealthChecksUI();

            //mysql
            //多个数据库上下文可以使用池减少开销，略微增加性能
            services.AddDbContext<BaseEduContext>(options =>
                options.UseLazyLoadingProxies().UseMySql(
                    Configuration.GetConnectionString("DefaultConnection"),
                    //弹性连接,命令超时
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure().CommandTimeout(3)));
            //PostGre
            //services.AddDbContextPool<BaseEduContext>(options =>
            //    options.UseLazyLoadingProxies().UseNpgsql(
            //        Configuration.GetConnectionString("PostGreSQLConnection"),
            //        //弹性连接,命令超时
            //        mySqlOptions => mySqlOptions.EnableRetryOnFailure().CommandTimeout(3)));

            //
            services.AddTransient(typeof(IAsyncRepository<>), typeof(SugarRepository<>));
            services.AddTransient(typeof(IRepository<>), typeof(SugarRepository<>));
            services.AddScoped<IAccount, Account>();
            services.AddSingleton<IEsClientProvider, EsClientProvider>();
            services.AddMediatR(typeof(PingHandler).Assembly,
                                typeof(Pong1).Assembly, typeof(Pong2).Assembly);
            services.AddSingleton<RpcClient>();
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<RedisSetting>>().Value;
                //也可以直接使用Configuration获取redis连接信息
                var configuration = ConfigurationOptions.Parse(settings.RedisConnectionString, true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            });
            //services.AddHostedService<TimedHostedService>();
            services.AddHostedService<RabbitHostedService>();

            JWTTokenOptions jwtTokenOptions = new JWTTokenOptions();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x =>
                {
                    x.LoginPath = new PathString("/Account/Login");
                    x.ExpireTimeSpan= new TimeSpan(0, 0, 30, 0, 0);
                    //x.CookieSecure = CookieSecurePolicy.None;
                    x.Cookie.SecurePolicy = CookieSecurePolicy.None;
                    //x.AccessDeniedPath = "";
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role,

                        //颁发机构
                        ValidIssuer = "https://localhost:44343/",
                        //颁发给谁
                        ValidAudience = "api",
                        //签名秘钥
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Consts.Secret))
                   
                    };
                });

            //
            services.AddControllersWithViews(options =>
            {
                //options.RespectBrowserAcceptHeader = true; // false by default

                //options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                //options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(ValidateModelStateFilter));
            })
            //忽略循环引用
            //.AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            //services.AddSwaggerGenNewtonsoftSupport();
            services.AddCors(options => 
            {
                options.AddPolicy("janus",p => p.AllowAnyOrigin());
            });

            // https://github.com/aspnet/Hosting/issues/793
            // the IHttpContextAccessor service is not registered by default.
            // the clientId/clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddSignalR();
            //使用autofac替换容器后，启动速度会慢很多。
            services.AddOptions();
            //var container = new ContainerBuilder();
            //container.Populate(services);
            ////向容器注入服务示例
            ////container.RegisterType<Account>().AsSelf().As<IAccount>().InstancePerLifetimeScope();
            ////container.RegisterGeneric(typeof(SugarRepository<>)).As(typeof(IRepository<>));
            //return new AutofacServiceProvider(container.Build());
        }
        private async Task Echo(Microsoft.AspNetCore.Http.HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            //NLog.LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("DefaultConnection"); Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  //避免日志中的中文输出乱码
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //反向代理
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            //app.UseHttpsRedirection();
            //实现非静态文件根目录的支持
            //var provider = new FileExtensionContentTypeProvider();
            //provider.Mappings.Remove(".exe");
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"File")),
            //    RequestPath = new PathString("/File"),
            //    OnPrepareResponse = ctx =>
            //    {
            //        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
            //    },
            //    ContentTypeProvider = provider
            //});
            app.UseStaticFiles();
            //
            app.UseCookiePolicy();
            app.UseAuthentication();

            //HealthyChecks
            //app.UseHealthChecks("/health",
            //    new HealthCheckOptions
            //    {
            //        ResponseWriter = async (context, report) =>
            //        {
            //            var result = JsonConvert.SerializeObject(
            //                new
            //                {
            //                    status = report.Status.ToString(),
            //                    errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
            //                });
            //            context.Response.ContentType = MediaTypeNames.Application.Json;
            //            await context.Response.WriteAsync(result);
            //        }
            //    });
            //app.UseHealthChecksUI();//
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //app.UseIpRateLimiting();

            //中间件测试
            app.UseRequestUser();
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/ws")
            //    {
            //        if (context.WebSockets.IsWebSocketRequest)
            //        {
            //            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //            await Echo(context, webSocket);
            //        }
            //        else
            //        {
            //            context.Response.StatusCode = 400;
            //        }
            //    }
            //    else
            //    {
            //        await next();
            //    }

            //});
            app.UseCors("janus");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "areaname",
                    pattern: "{Admin:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}");

                endpoints.MapHub<ChatHub>("/chatHub", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling;
                });
            });
        }


        //public void ConfigureContainer(ContainerBuilder builder)
        //{
        //    builder.RegisterAssemblyTypes(typeof(Program).Assembly).
        //        Where(x => x.Name.EndsWith("service", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();
        //    builder.RegisterDynamicProxy();
        //}
    }
}
