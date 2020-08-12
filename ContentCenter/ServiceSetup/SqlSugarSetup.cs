using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.ServiceSetup
{
    public static class SqlSugarSetup
    {
        public static void ConfigSqlSugar(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

        //    List<ConnectionConfig> ccList = new List<ConnectionConfig>();

            var appcfg = configuration.GetSection("DataBases").GetChildren();

            foreach (var cfg in appcfg)
            {
                ConnectionConfig cc = new ConnectionConfig
                {
                    DbType = (DbType)cfg["DbType"].ObjToInt(),
                    ConfigId = cfg["ConfId"],
                    IsAutoCloseConnection = true,
                    
                    IsShardSameThread = true,
                    ConnectionString = cfg["Connection"]
                };
                services.AddScoped<ISqlSugarClient>(o =>
                {
                    return new SqlSugarClient(cc);
                });
                //  ccList.Add(cc);
            }
            
            //services.AddScoped<ISqlSugarClient>(o =>
            //{
              

            //    return new SqlSugarClient(ccList);
            //});

        }

        public static void AddInitDbSeed(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var appcfg = configuration.GetSection("DataBases").GetChildren();

            foreach (var cfg in appcfg)
            {
                ConnectionConfig cc = new ConnectionConfig
                {
                    DbType = (DbType)cfg["DbType"].ObjToInt(),
                    ConfigId = cfg["ConfId"],
                    IsAutoCloseConnection =true,
                    IsShardSameThread = true,
                    ConnectionString = cfg["Connection"],
                    InitKeyType = InitKeyType.Attribute,
                };
                services.AddScoped<SqlSugarClient>(o =>
                {
                    return new SqlSugarClient(cc);
                });
                //  ccList.Add(cc);
            }

        }
    }
}
