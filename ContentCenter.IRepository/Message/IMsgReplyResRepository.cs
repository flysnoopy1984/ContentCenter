using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IRepository
{
    public interface IMsgReplyResRepository : IBaseRepository<EMsgInfo_ReplyRes>
    {
        EMsgContent_ReplyRes GetContentReplyRes_Sync(long commentId, long replyId);


        long AddContentReplyRes_Sync(EMsgContent_ReplyRes content);
    }
}
