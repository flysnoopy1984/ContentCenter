using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface ICommentReplyRepository:IBaseRepository<ECommentReply_Res>
    {
        Task<bool> DeleteAllReplyByCommentId(long commentId);

        Task<ModelPager<VueCommentReply>> GetReplysByCommentId(QComment_Reply query);

        Task<ModelPager<VueUserCommReply>> queryUserCommReply(QUserCommReply query);

        /// <summary>
        /// 根据Id获取发布者UserId(发消息使用)
        /// ID headerUrl Name
        /// </summary>
        /// <returns></returns>
        EUserInfo getReplyAutherId(long replyId);
    }
}
