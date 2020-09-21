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
            //db.CodeFirst.InitTables<EUserInfo>();
            // db.CodeFirst.InitTables<EUserWeixin>();
            //db.CodeFirst.InitTables<EResourceInfo>();
            //db.CodeFirst.InitTables<ESearchKeyLog>();
            //db.CodeFirst.InitTables<EResourceInfo>();
            //db.CodeFirst.InitTables<EResourceRequire_Log>();
            //db.CodeFirst.InitTables<ESysConfig>();
            db.CodeFirst.InitTables<EUserChargeTrans>();
            db.CodeFirst.InitTables<EUserPointsTrans>();
            db.CodeFirst.InitTables<EUserCommissionTrans>();
            db.CodeFirst.InitTables<EUserBalanceTrans>();
            //db.CodeFirst.InitTables<EUserChargeTrans>();

            Console.WriteLine("End InitDb");
        }
    }
}
