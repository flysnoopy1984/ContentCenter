using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.ServiceSetup
{
    public static class SwaggerSetup
    {
        #region Swagger 变量
        public const string ApiVer = "1.0";
        public const string ApiName  = "内容中心";

        #endregion


        public static void ConfigSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc(ApiVer, new OpenApiInfo
                {
                    Version = ApiVer,
                    Title = $"{ApiName} 接口文档",
                    Description = $"{ApiName} HTTP API {ApiVer}",
                    Contact = new OpenApiContact { Name = ApiName, Email = "395940187@qq.com", Url = new Uri("https://www.jianshu.com/u/c5606e322e7e") },
                    License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.jianshu.com/u/c5606e322e7e") }
                });
                c.OrderActionsBy(o => o.RelativePath);

                var xmlPath = Path.Combine(AppContext.BaseDirectory, "ContentCenter.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                var xmlModelPath = Path.Combine(AppContext.BaseDirectory, "ContentCenter.Model.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlModelPath);//默认的第二个参数是false，这个是controller的注释，记得修改

                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                // 必须是 oauth2
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

            });
        }
    }
}
