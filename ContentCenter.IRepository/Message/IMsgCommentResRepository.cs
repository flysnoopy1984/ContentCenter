using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IMsgCommentResRepository:IBaseRepository<EMsgInfo_CommentRes>
    {
        EMsgContent_CommentRes GetContentCommentRes_Sync(string recCode);

      //  bool ExistMsgCommentRes_Sync(string resCode, string sendUserId);

        long AddContentCommentRes_Sync(EMsgContent_CommentRes content);

        //查询用户评论消息
        Task<ModelPager<VueMsgInfoNotification>> queryUserComment(QMsgUser query);

        //消息更新
        int UpdateMsgStatus(SubmitUnReadMsgIdList submitData);
    }
}
