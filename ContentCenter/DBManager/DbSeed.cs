using Autofac.Extras.DynamicProxy;
using ContentCenter.AOP;
using ContentCenter.Model;
using ContentCenter.Model.Users;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.DBManager
{
   
    public class DbSeed
    {

       
        public static void InitDb(ISqlSugarClient db)
        {
            Console.WriteLine("Start InitDb");
            db.DbMaintenance.CreateDatabase(databaseName: "MasterCrawlerData");
           
            db.CodeFirst.InitTables<ESectionTag>();
            Console.WriteLine("End InitDb");
        }
    }
}
