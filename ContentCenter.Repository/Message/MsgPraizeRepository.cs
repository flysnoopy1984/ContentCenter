using ContentCenter.IRepository;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class MsgPraizeRepository : BaseRepository<EMsgInfo_Praize>, IMsgPraizeRepository
    {
        public MsgPraizeRepository(ISqlSugarClient[] sugarClient)
         : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        #region Message
        public bool ExistMsgPraize_Sync(string refId, PraizeTarget praizeTarget, string sendUserId)
        {
            var r = base.GetCount(
                c => c.RefId == refId &&
                c.PraizeTarget == praizeTarget &&
                c.SendUserId == sendUserId 
             //   c.NotificationStatus == NotificationStatus.
                ).Result;

            return r > 0;
        }


        public async Task<ModelPager<VueMsgInfoNotification>> queryUserPraize(QMsgUser query)
        {
            ModelPager<VueMsgInfoNotification> result = new ModelPager<VueMsgInfoNotification>(query.pageIndex, query.pageSize);
            var joinSql = Db.Queryable<EMsgInfo_Praize, EMsgContent_Praize>((m, c) => new object[]
            {
                JoinType.Inner,m.RefId == c.RefId &&
                m.PraizeTarget == c.PraizeTarget,
            });
           
            var mainSql = joinSql.OrderBy((m,c)=>m.CreatedDateTime,OrderByType.Desc)
            .Where((m,c)=>m.ReceiveUserId== query.userId)
            .Select((m,c) => new VueMsgInfoNotification
            {
                dbDateTime = m.CreatedDateTime,
                senderHeaderUrl = m.SendHeaderUrl,
                senderId = m.SendUserId,
                senderName = m.SendName,
                bookCode = c.BookCode,
                bookUrl = c.BookUrl,
                resCode = c.ResCode,
                resName = c.ResName,
                commentId = c.CommentId, //20201029 用于置顶
                replyId = c.ReplyId,  //20201029 用于置顶
                origContent =c.OrigContent,
                receiveContent = "",
                target = m.PraizeTarget,   
                
                NotificationStatus = m.NotificationStatus,
                msgId = m.Id
            });

            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;
            return result;
        }


        #endregion

        #region Content

        public EMsgContent_Praize GetContentPraize_Sync(string refId, PraizeTarget praizeTarget)
        {
            return Db.Queryable<EMsgContent_Praize>().Where(
                c => c.RefId == refId &&
              c.PraizeTarget == praizeTarget
              ).First();

        }

       
        public long AddContentPraize_Sync(EMsgContent_Praize content)
        {
            var insertable = Db.Insertable<EMsgContent_Praize>(content);
            return insertable.ExecuteReturnBigIdentity();
        }

        public int UpdateMsgStatus(SubmitUnReadMsgIdList submitData)
        {
          //  Console.WriteLine("DB: updateMsg start");
            var op = Db.Updateable<EMsgInfo_Praize>()
              .SetColumns(m => new EMsgInfo_Praize
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
            catch(Exception ex)
            {
                var m = ex;
                throw ex;
            }

            // return 
        }



        #endregion




    }
}
