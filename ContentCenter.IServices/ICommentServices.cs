using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IServices
{
    public interface ICommentServices: IBaseServices<EComment_Res>
    {
        /// <summary>
        /// 提交评论，如果有点赞提交点赞
        /// </summary>
        /// <param name="commentRes"></param>
        long submitResComment(SubmitComment submitComment);

        long submitCommentReply(SubmitReply submitComment);
        /// <summary>
        /// 删除资源的评论 包括点赞 和回复 和 回复点赞 
        /// </summary>
        /// <param name="commentId"></param>
        void deleteComment_Res(long commentId);

        /// <summary>
        /// 删除评论回复 包括点赞 更新回复总数
        /// </summary>
        /// <param name="replyId"></param>
        void deleteCommentReply(long replyId,long commentId);

        /// <summary>
        /// 加载资源评论
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ModelPager<VueCommentInfo> loadMoreComment_Res(QComment_Res query);

        /// <summary>
        /// 加载评论回复
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        ModelPager<VueCommentReply> loadMoreComment_Reply(QComment_Reply query);
    }
}
