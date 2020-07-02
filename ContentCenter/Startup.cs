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
                      options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;  // ����ʱ��Ϊ UTC)
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

            #region AOP������
            builder.RegisterType<CCLogAOP>();
            #endregion

            #region ����ע��
            builder.RegisterType<UserServices>()
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(CCLogAOP));

            builder.RegisterType<AdminServices>()
               .InstancePerLifetimeScope()
               .AsImplementedInterfaces();


            builder.RegisterType<BookServices>()
               .InstancePerLifetimeScope()
               .AsImplementedInterfaces()
               .EnableInterfaceInterceptors()
               .InterceptedBy(typeof(CCLogAOP));

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

            // ������֤
            app.UseAuthentication();

            // ��Ȩ�м��
            app.UseAuthorization();

            #region Swagger
            if (env.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {

                    c.SwaggerEndpoint($"/swagger/{SwaggerSetup.ApiVer}/swagger.json", $"{SwaggerSetup.ApiName} {SwaggerSetup.ApiVer}");

                    //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ����������뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "doc";
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
