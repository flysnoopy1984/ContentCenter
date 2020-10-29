using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface ICommentRepository: IBaseRepository<EComment_Res>
    {

        // 根据资源获取评论页
        // fixedCommentId 固定的评论
        Task<ModelPager<VueCommentInfo>> GetCommentsByResCodes(QComment_Res query);

        /// <summary>
        /// 更新评论的总回复数
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        bool UpdateComment_ReplyNum(long commentId,OperationDirection direction,int num=1);

        Task<ModelPager<VueUserComm>> queryUserComm(QUserComm query);

        /// <summary>
        /// 根据Id获取发布者(发消息使用)
        /// ID headerUrl Name
        /// </summary>
        /// <returns></returns>
        EUserInfo getCommentAutherId(long commentId);

   
    }
}
