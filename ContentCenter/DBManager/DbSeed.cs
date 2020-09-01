using Autofac.Extras.DynamicProxy;
using ContentCenter.AOP;
using ContentCenter.Model;
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
            //db.DbMaintenance.CreateDatabase(databaseName: "ContentCenter");
            //db.CodeFirst.InitTables<EComment_Res>();
            //db.CodeFirst.InitTables<ECommentReply_Res>();
            //db.CodeFirst.InitTables<EPraize_Res>();
            //db.CodeFirst.InitTables<EPraize_Comment>();
            //db.CodeFirst.InitTables<EPraize_CommentReply>();
           db.CodeFirst.InitTables<EComment_Res>();
            //db.CodeFirst.InitTables<EResourceInfo>();
            //db.CodeFirst.InitTables<EUserInfo>();
            //db.CodeFirst.InitTables<ESearchKeyLog>();
            //db.CodeFirst.InitTables<EResourceInfo>();
            //db.CodeFirst.InitTables<EResourceRequire_Log>();
            Console.WriteLine("End InitDb");
        }
    }
}
