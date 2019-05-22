# EduTest
EduTest

目前实现如下功能：    
orm：sugarsql， ef core（mysql）；   
tools：常用的基本方法；  
认证：mvc使用identity的cookie，api使用jwt（颁发和认证）；  
视图：提供ViewComponent的实例替换以前的动态视图；    
日志：使用nlog，支持file，debugger，datebase，ElasticSearch；    
ElasticSearch：提供插入和查询的示例，使用nest作为客户端，服务端使用java 1.8和最新版的ElasticSearch window客户端,Kibana可视化；  
api：提供swagger支持；  
后台管理：基本的用户，角色及权限控制后台逻辑全部完成（以前旧项目，找时间会重写，细化逻辑）；  
docker：提供docker启动，端口部分需要自配；  
redis：ConnectionMultiplexer的单例注入根据需求自配configuration；   
MediatR：进程内部通信，用于领域事件；   
过滤：HttpGlobalExceptionFilter，ValidateModelStateFilter；    

待开发功能：  
授权，接入id4；  
ElasticSearch：增加全文数据检索的示例项目（2000万以上数据），提供日志和全文数据的清洗，linux环境下部署；    
api：为满足部分需求，接入oauth 2；  
后台管理：angular在研究中，前端后台管理考虑使用ng-alain，并提供taghelper支持；  
实时通信：ASP.NET Core SignalR，针对部分移动设备通信问题，增加socket支持，消息队列；  
测试：xunit，集成测试；  
httpcilentHelper：优化部分封装，微信公众号的开发作为示例项目，结合多个功能；  
支付接口：；  
linux：部署文档，常见问题；  
校验：FluentValdiation；  
映射工具：AutoMapper；   
异常处理：增加错误页模板，添加mvc过滤器和相关重定位；  
apigetway：Ocelot；  
通信：DotNetty，参照netty4.X java示例和相关文档学习，后续提供IM示例；  
消息队列：rabbitmq；  

待接入功能：  
流媒体：ffmpeg推流，推流鉴权，视频剪切；  
rtc：针对现有的easytrc项目，turnserver调整，增加ubuntu下的守护进程；  


