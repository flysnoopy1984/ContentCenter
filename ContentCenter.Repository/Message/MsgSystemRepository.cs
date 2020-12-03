using ContentCenter.IRepository;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCenter.Repository
{
    public class MsgSystemRepository : BaseRepository<EMsgInfo_System>, IMsgSystemRepository
    {
        public MsgSystemRepository(ISqlSugarClient[] sugarClient)
         : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public void AddContentSystem(EMsgContent_System content)
        {
            Db.Insertable(content).ExecuteCommand();
        }

        public EMsgContent_System GetContentSystem_Sync(long Id)
        {
           
           return Db.Queryable<EMsgContent_System>().Where(
                c=>c.Id == Id
           ).First();
        }

        public ModelPager<VueSystemNotification> querySystemNotification(QMsgUser query)
        {
            ModelPager<VueSystemNotification> result = new ModelPager<VueSystemNotification>(query.pageIndex, query.pageSize);
            var mainSql = Db.Queryable<EMsgInfo_System, EMsgContent_System>((m, c) => new object[]
            {
                JoinType.Left,m.ContentId == c.Id,

            })
            .Where((m, c) => m.ReceiveUserId == query.userId)
            .OrderBy((m, c) => m.CreatedDateTime, OrderByType.Desc)
            .Select((m, c) => new VueSystemNotification
            {
                dbDateTime = m.CreatedDateTime,
                htmlContent = c.htmlContent,
                htmlTitle = c.htmlTitle,
                NotificationStatus = m.NotificationStatus,
                msgId = m.Id,
                contentId = c.Id
            });

            int totalNumber =0 ;
            result.datas = mainSql.ToPageList(query.pageIndex, query.pageSize,ref totalNumber);
            result.totalCount = totalNumber;
            return result;
        }

        public int UpdateMsgStatus(SubmitUnReadMsgIdList submitData)
        {
            var op = Db.Updateable<EMsgInfo_System>()
             .SetColumns(m => new EMsgInfo_System
             {
                 NotificationStatus = submitData.targetStatus
             })
             .Where(m => m.ReceiveUserId == submitData.userId && submitData.msgIdList.Contains(m.Id)
             && m.NotificationStatus != NotificationStatus.read);
            try
            {
                var r = op.ExecuteCommand();
                //  Console.WriteLine("DB: updateMsg end");
                return r;
            }
            catch (Exception ex)
            {
                var m = ex;
                throw ex;
            }
        }
    }
}
