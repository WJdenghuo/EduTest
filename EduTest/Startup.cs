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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector.Logging;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EduTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }
        private readonly ILogger _logger;
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
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

            //mysql
            //多个数据库上下文可以使用池减少开销，略微增加性能
            services.AddDbContext<BaseEduContext>(options =>
                options.UseLazyLoadingProxies().UseMySql(
                    Configuration.GetConnectionString("DefaultConnection"),
                    //弹性连接,命令超时
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure().CommandTimeout(3)));
            
            //
            services.AddTransient(typeof(IAsyncRepository<>), typeof(SugarRepository<>));
            services.AddTransient(typeof(IRepository<>), typeof(SugarRepository<>));
            services.AddScoped<IAccount, Account>();
            services.AddSingleton<IEsClientProvider, EsClientProvider>();
            services.AddMediatR(typeof(PingHandler).Assembly,
                                typeof(Pong1).Assembly, typeof(Pong2).Assembly);
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<RedisSetting>>().Value;
                //也可以直接使用Configuration获取redis连接信息
                var configuration = ConfigurationOptions.Parse(settings.RedisConnectionString, true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddHostedService<TimedHostedService>();

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
            services.AddMvc(options =>
            {
                //options.RespectBrowserAcceptHeader = true; // false by default

                //options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                //options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(ValidateModelStateFilter));
            })
            //忽略循环引用
            //.AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

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
            var container = new ContainerBuilder();
            container.Populate(services);
            //向容器注入服务示例
            //container.RegisterType<Account>().AsSelf().As<IAccount>().InstancePerLifetimeScope();
            //container.RegisterGeneric(typeof(SugarRepository<>)).As(typeof(IRepository<>));
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCors("janus");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "areaname",
                    template: "{Admin:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapAreaRoute(
                    name: "Admin",
                    areaName: "Admin",
                    template: "Admin/{controller=Home}/{action=Index}");
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
