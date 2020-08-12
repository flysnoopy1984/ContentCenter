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
        Task<long> AddPraize_Comment(EPraize_Comment praize);

        Task<long> AddPraize_CommentReply(EPraize_CommentReply praize);

        Task<bool> DeletePraized_Res(string resCode, string userId);

        /// <summary>
        /// 更新点赞表本身，原来好评的便差评（只在资源中）
        /// </summary>
        Task<bool> UpdatePraized_Res(PraizeType praizeType, string resCode, string userId);

        Task<EPraize_Res> GetPraize_Res(string resCode, string userId);

        /// <summary>
        /// 更新资源主表点赞总数量
        /// </summary>
        /// <param name="direction">有差评和好评，所以有更新操作存在</param>
        Task<bool> UpdateResPraizedNum(string resCode, PraizeType praizeType, OperationDirection direction);

        /// <summary>
        /// 更新评论表点赞总数量（只统计好评）
        /// </summary>
        Task<bool> UpdateCommentPraized_GoodNum(long commentId, OperationDirection direction);

        /// <summary>
        /// 更新评论表点赞总数量（只统计好评）
        /// </summary>
        Task<bool> UpdateCommentReplyPraized_GoodNum(long commentReplyId, OperationDirection direction);

        Task<bool> DeletePraized_Comment_Res(long commentId, string userId);

        Task<bool> DeletePraized_CommentReply_Res(long commentId, string userId);

        Task<int> HasPraized_Res(string resCode,string userId);

        Task<int> HasPraized_Comment_Res(long commentId, string userId);

        Task<int> HasPraized_CommentReply_Res(long commentReplyId, string userId);

     
    }
}
