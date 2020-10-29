using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IMsgReplyResRepository : IBaseRepository<EMsgInfo_ReplyRes>
    {
      //  EMsgContent_ReplyRes GetContentReplyRes_Sync(long commentId, long replyId);
        EMsgContent_ReplyRes GetContentReplyRes_Sync(string resCode);

        long AddContentReplyRes_Sync(EMsgContent_ReplyRes content);

        Task<ModelPager<VueMsgInfoNotification>> queryUserReply(QMsgUser query);

        //更新消息为已读
        int UpdateMsgStatus(SubmitUnReadMsgIdList submitData);
    }
}
