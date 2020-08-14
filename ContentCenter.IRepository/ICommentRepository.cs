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
        /// <summary>
        /// 根据资源获取评论页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ModelPager<VueCommentInfo>> GetCommentsByResCodes(QComment_Res query);

        /// <summary>
        /// 更新评论的总回复数
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        bool UpdateComment_ReplyNum(long commentId,OperationDirection direction,int num=1);

    }
}
