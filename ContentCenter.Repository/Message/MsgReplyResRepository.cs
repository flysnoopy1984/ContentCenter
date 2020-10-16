using ContentCenter.IRepository;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public EMsgContent_ReplyRes GetContentReplyRes_Sync(long commentId,long replyId)
        {
            return Db.Queryable<EMsgContent_ReplyRes>().Where(
              c => c.CommentId == commentId &&
              c.ReplyId == replyId)
            .First();
        }
    }
}
