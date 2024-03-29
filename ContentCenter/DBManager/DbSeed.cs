﻿using Autofac.Extras.DynamicProxy;
using ContentCenter.AOP;
using ContentCenter.Model;
using ContentCenter.Model.ThirdPart.Baidu;
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
              db.CodeFirst.InitTables<EBookInfo>();
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

            //db.CodeFirst.InitTables<EUserChargeTrans>();
            //db.CodeFirst.InitTables<EUserPointsTrans>();
            //db.CodeFirst.InitTables<EUserCommissionTrans>();
            //db.CodeFirst.InitTables<EUserBalanceTrans>();
            //db.CodeFirst.InitTables<EUserChargeTrans>();

            //  db.CodeFirst.InitTables<EMsgContent_Praize>();
            //db.CodeFirst.InitTables<EMsgContent_CommentRes>();
            //db.CodeFirst.InitTables<EMsgContent_ReplyRes>();

            //db.CodeFirst.InitTables<EMsgInfo_CommentRes>();
            // db.CodeFirst.InitTables<EMsgInfo_Praize>();
            //db.CodeFirst.InitTables<EMsgInfo_ReplyRes>();
            //db.CodeFirst.InitTables<EMsgInfoOverview>();
            //db.CodeFirst.InitTables<EMsgInfo_System>();
            //  db.CodeFirst.InitTables<EMsgContent_System>();
         //   db.CodeFirst.InitTables<panBookInfo>();
         //   db.CodeFirst.InitTables<ESection>();
        //    db.CodeFirst.InitTables<panBook_DouBan_Relation>();
            Console.WriteLine("End InitDb");
        }
    }
}
