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
    public class MsgReplyResRepository: BaseRepository<EMsgInfo_ReplyRes>,IMsgReplyResRepository
    {
        public MsgReplyResRepository(ISqlSugarClient[] sugarClient)
           : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public long AddContentReplyRes_Sync(EMsgContent_ReplyRes content)
        {
            var insertable = Db.Insertable(content);
            return insertable.ExecuteReturnBigIdentity();
        }
        public EMsgContent_ReplyRes GetContentReplyRes_Sync(string resCode)
        {
            return Db.Queryable<EMsgContent_ReplyRes>().Where(
             c=>c.ResCode == resCode)
            .First();
        }

        public int UpdateMsgStatus(SubmitUnReadMsgIdList submitData)
        {
            var op = Db.Updateable<EMsgInfo_ReplyRes>()
            .SetColumns(m => new EMsgInfo_ReplyRes
            {
                NotificationStatus = submitData.targetStatus
            })
            .Where(m => m.ReceiveUserId == submitData.userId && submitData.msgIdList.Contains(m.Id)
            && m.NotificationStatus != NotificationStatus.read);
            try
            {
                var r = op.ExecuteCommand();

                return r;
            }
            catch (Exception ex)
            {
                var m = ex;
                throw ex;
            }
        }

        public async Task<ModelPager<VueMsgInfoNotification>> queryUserReply(QMsgUser query)
        {
            ModelPager<VueMsgInfoNotification> result = new ModelPager<VueMsgInfoNotification>(query.pageIndex, query.pageSize);
            var mainSql = Db.Queryable<EMsgInfo_ReplyRes, EMsgContent_ReplyRes>((m, c) => new object[]
            {
                JoinType.Left,m.resCode == c.ResCode

            })
            .Where((m, c) => m.ReceiveUserId == query.userId)
             .OrderBy((m, c) => m.CreatedDateTime, OrderByType.Desc)
            .Select((m, c) => new VueMsgInfoNotification
            {
                dbDateTime = m.CreatedDateTime,
                senderHeaderUrl = m.SendHeaderUrl,
                senderId = m.SendUserId,
                senderName = m.SendName,
                bookCode = c.BookCode,
                bookUrl = c.BookUrl,
                resCode = c.ResCode,
                resName = c.ResName,
                commentId = m.CommentId,
                replyId = m.ReplyId,
                receiveContent = m.ReceiveContent,
                NotificationStatus = m.NotificationStatus,
                msgId = m.Id
            });

            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;
            return result;
        }
    }
}
