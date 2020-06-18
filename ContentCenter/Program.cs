using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using ContentCenter.DBManager;
using ContentCenter.Model.Users;
using ContentCenter.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace ContentCenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .Build();
            try
            {
                RunExternalCode(host);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in RunExternalCode:{ex.Message}");
            }

            host.Run();
           
        }

        public static void RunExternalCode(IHost host)
        {
            /* 启动时初始化数据库（如果需要） */
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                
                bool needDbInit =Convert.ToBoolean(config.GetSection("InitTask")["NeedDbInit"]);
                if(needDbInit)
                {
                   var db = services.GetServices<SqlSugarClient>().FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.BookDbKey);
                  //   ISqlSugarClient sqlsugarClient = services.GetRequiredService<SqlSugarClient>();
                    DbSeed.InitDb(db);
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                   .ConfigureKestrel(serverOptions =>
                   {
                       serverOptions.AllowSynchronousIO = true;//启用同步 IO
                    })
                   .UseStartup<Startup>();
                   // webBuilder.UseStartup<Startup>();
                });
    }
}
