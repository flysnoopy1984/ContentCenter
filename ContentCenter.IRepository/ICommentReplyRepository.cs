using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface ICommentReplyRepository:IBaseRepository<ECommentReply_Res>
    {
        Task<bool> DeleteAllReplyByCommentId(long commentId);
    }
}
