﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookTasks.setup
{
    public static class SqlSugerSetup
    {
        public static void AddSqlSugarSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<ISqlSugarClient>(o =>
            {
                List<ConnectionConfig> ccList = new List<ConnectionConfig>();

                var appcfg = configuration.GetSection("DataBases").GetChildren();

                foreach (var cfg in appcfg)
                {
                    ConnectionConfig cc = new ConnectionConfig
                    {
                        DbType = (DbType)cfg["DbType"].ObjToInt(),
                        ConfigId = cfg["ConfId"],
                        IsAutoCloseConnection = true,
                        IsShardSameThread = true,
                        ConnectionString = cfg["Connection"],

                    };
                    ccList.Add(cc);
                }

                var db = new SqlSugarClient(ccList);

                return db;
            });

        }

    }
}
