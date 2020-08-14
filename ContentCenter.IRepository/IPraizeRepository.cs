using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IPraizeRepository: IBaseRepository<EPraize_Res>
    {
        long AddPraize_Comment(EPraize_Comment praize);

        long AddPraize_CommentReply(EPraize_CommentReply praize);

        bool DeletePraized_Res(string resCode, string userId);

        /// <summary>
        /// 更新点赞表本身，原来好评的便差评（只在资源中）
        /// </summary>
        bool UpdatePraized_Res(PraizeType praizeType, string resCode, string userId);

        Task<EPraize_Res> GetPraize_Res(string resCode, string userId);

        /// <summary>
        /// 更新资源主表点赞总数量
        /// </summary>
        /// <param name="direction">有差评和好评，所以有更新操作存在</param>
        bool UpdateResPraizedNum(string resCode, PraizeType praizeType, OperationDirection direction);

        /// <summary>
        /// 更新评论表点赞总数量（只统计好评）
        /// </summary>
        bool UpdateCommentPraized_GoodNum(long commentId, OperationDirection direction,int num=1);

        /// <summary>
        /// 更新评论表点赞总数量（只统计好评）
        /// </summary>
        bool UpdateCommentReplyPraized_GoodNum(long commentReplyId, OperationDirection direction,int num=1);

        bool DeletePraized_Comment_Res(long commentId, string userId);

        int DeletePraized_CommentReply_Res(long replyId, string userId);

        /// <summary>
        /// 删除评论下所有回复的点赞
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        int DeletePraized_AllReplyBelowComment(long commentId);

        Task<int> HasPraized_Res(string resCode,string userId);

        Task<int> HasPraized_Comment_Res(long commentId, string userId);

        Task<int> HasPraized_CommentReply_Res(long commentReplyId, string userId);

     
    }
}
