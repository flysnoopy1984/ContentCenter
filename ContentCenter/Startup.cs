using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using ContentCenter.AOP;
using ContentCenter.AuthManager;
using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model.Commons;
using ContentCenter.Repository;
using ContentCenter.Services;
using ContentCenter.ServiceSetup;
using IQB.Util.Models.Oss;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ContentCenter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        public IConfiguration Configuration { get; }

      
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                  .AddNewtonsoftJson(options =>
                  {
                      options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                      options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;  // 设置时区为 UTC)
                      options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                  });
            services.ConfigSqlSugar(Configuration);
            services.AddInitDbSeed(Configuration);

            #region Swagger
            services.ConfigSwagger(Configuration);

            #endregion

            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IUserServices, UserServices>();

            #region Id4(replaced JWT)
            //   services.AddJWTAuth(Configuration);
            services.ConfigId4Auth(Configuration);
            #endregion

            #region  CROS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowedRequest", p =>
                {
                     var sec = new string[] { "ApiSetting", "Cors", "IPs" };
                  
                    var origns = Configuration[string.Join(":", sec)].Split(',');
                     p.WithOrigins(origns)
                      .AllowAnyHeader()//Ensures that the policy allows any header.
                      .AllowAnyMethod();
                });
            });
            #endregion

            #region MemoryCache
            services.AddMemoryCache();
            #endregion
        }

        //autoFac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region DB
            //List<ConnectionConfig> ccList = new List<ConnectionConfig>();

            //var appcfg =Configuration.GetSection("DataBases").GetChildren();

            //foreach (var cfg in appcfg)
            //{
            //    ConnectionConfig cc = new ConnectionConfig
            //    {
            //        DbType = (DbType)cfg["DbType"].ObjToInt(),
            //        ConfigId = cfg["ConfId"],
            //        IsAutoCloseConnection = true,
            //        ConnectionString = cfg["Connection"] 
            //    };
            //    ccList.Add(cc);    
            //}
            //builder.RegisterInstance<ISqlSugarClient>(new SqlSugarClient(ccList))
            //    .InstancePerLifetimeScope();
            #endregion

            #region AOP拦截器
           // builder.RegisterType<CCLogAOP>();
            #endregion

            #region 实例注入
            builder.RegisterInstance(new OssConfig
            {
                accessKeyId = Configuration["ossConfig:accessKeyId"],
                accessKeySecret = Configuration["ossConfig:accessKeySecret"],
                bucketName = Configuration["ossConfig:bucketName"],
                endpoint = Configuration["ossConfig:endpoint"],
            }) ;
            #endregion

            #region 服务注入
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            var servicesDllFile = Path.Combine(basePath, "ContentCenter.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "ContentCenter.Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                throw new Exception("Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。");
            }
            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerDependency();
                      //.EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                      //.InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            /*   builder.RegisterType<UserServices>()
                   .InstancePerLifetimeScope()
                   .AsImplementedInterfaces();
                  // .EnableInterfaceInterceptors()
                 //  .InterceptedBy(typeof(CCLogAOP));

               builder.RegisterType<AdminServices>()
                  .InstancePerLifetimeScope()
                  .AsImplementedInterfaces();

               builder.RegisterType<ResourceServices>()
                 .InstancePerLifetimeScope()
                 .AsImplementedInterfaces();

               builder.RegisterType<BookServices>()
                  .InstancePerLifetimeScope()
                  .AsImplementedInterfaces();
               builder.RegisterType<SearchServices>()
                 .InstancePerLifetimeScope()
                 .AsImplementedInterfaces();
               //.EnableInterfaceInterceptors()
               //.InterceptedBy(typeof(CCLogAOP));

               builder.RegisterType<BookRepository>()
                   .InstancePerLifetimeScope()
                   .AsImplementedInterfaces();
               builder.RegisterType<SectionRepository>()
               .InstancePerLifetimeScope()
               .AsImplementedInterfaces();
               builder.RegisterType<UserRepository>()
                  .InstancePerLifetimeScope()
                  .AsImplementedInterfaces();
               builder.RegisterType<AdminRepository>()
                  .InstancePerLifetimeScope()
                  .AsImplementedInterfaces();
               builder.RegisterType<CommentRepository>()
                 .InstancePerLifetimeScope()
                 .AsImplementedInterfaces();
               builder.RegisterType<ResourceRepository>()
                 .InstancePerLifetimeScope()
                 .AsImplementedInterfaces();
               builder.RegisterType<SearchRepository>()
               .InstancePerLifetimeScope()
               .AsImplementedInterfaces();
               */
            #endregion
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowedRequest");
            app.UseRouting();
            app.UseStatusCodePages();

            // 开启认证
            app.UseAuthentication();

            // 授权中间件
            app.UseAuthorization();

            #region Swagger
            if (env.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {

                    c.SwaggerEndpoint($"/swagger/{SwaggerSetup.ApiVer}/swagger.json", $"{SwaggerSetup.ApiName} {SwaggerSetup.ApiVer}");

                    //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                    c.RoutePrefix = "doc";
                });
            }
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           
        }
    }
}
