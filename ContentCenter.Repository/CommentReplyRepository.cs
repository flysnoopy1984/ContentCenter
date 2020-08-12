using ContentCenter.IRepository;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class CommentReplyRepository: BaseRepository<ECommentReply_Res>, ICommentReplyRepository
    {
        public CommentReplyRepository(ISqlSugarClient[] sugarClient)
           : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public Task<bool> DeleteAllReplyByCommentId(long commentId)
        {
            return base.DeleteRangeByExp(a => a.commentId == commentId);
        }
    }
}
