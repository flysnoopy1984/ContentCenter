using ContentCenter.IRepository;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class PraizeRepository : BaseRepository<EPraize_Res>,IPraizeRepository
    {
        public PraizeRepository(ISqlSugarClient[] sugarClient)
           : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }
     
        public Task<long> AddPraize_Comment(EPraize_Comment praize)
        {
            var insertable = Db.Insertable(praize);
            return insertable.ExecuteReturnBigIdentityAsync();
        }


        public Task<long> AddPraize_CommentReply(EPraize_CommentReply praize)
        {
            var insertable = Db.Insertable(praize);
            return insertable.ExecuteReturnBigIdentityAsync();
        }

        public Task<int> HasPraized_Comment_Res(long commentId, string userId)
        {
            return Db.Queryable<EPraize_Comment>().Where(a => a.commentId== commentId && a.userId == userId). CountAsync();
        }

        public Task<int> HasPraized_CommentReply_Res(long commentReplyId, string userId)
        {
            return Db.Queryable<EPraize_CommentReply>().Where(a => a.commentReplyId == commentReplyId && a.userId == userId).CountAsync();
        }

        public Task<int> HasPraized_Res(string resCode, string userId)
        {
            return base.GetCount(a => a.ResCode == resCode && a.userId == userId);
        }
        public Task<bool> UpdatePraized_Res(PraizeType praizeType, string resCode, string userId)
        {
           

            var op = Db.Updateable<EPraize_Res>().SetColumns(a => new EPraize_Res() { PraizeType = praizeType });
            op = op.Where(a => a.ResCode == resCode && a.userId == userId);

            return op.ExecuteCommandHasChangeAsync();
        }

        public Task<bool> DeletePraized_Res(string resCode, string userId)
        {
        
            return base.DeleteRangeByExp(a => a.ResCode == resCode && a.userId == userId);
        }

        public Task<EPraize_Res> GetPraize_Res(string resCode, string userId)
        {
            return GetByExpSingle(a => a.userId == userId && a.ResCode == resCode);
        }

        public Task<bool> DeletePraized_Comment_Res(long commentId, string userId)
        {
            var op = Db.Deleteable<EPraize_Comment>( a=>a.commentId == commentId && a.userId == userId);
            return  op.ExecuteCommandHasChangeAsync();
        }

        public Task<bool> DeletePraized_CommentReply_Res(long commentId, string userId)
        {
            var op = Db.Deleteable<EPraize_CommentReply>(a => a.commentId == commentId && a.userId == userId);
            return op.ExecuteCommandHasChangeAsync();
        }

        public Task<bool> UpdateResPraizedNum(string resCode, PraizeType praizeType, OperationDirection direction)
        {

            var op = Db.Updateable<EResourceInfo>().SetColumns(a => new EResourceInfo() { goodNum = a.goodNum - 1 });

            if(praizeType == PraizeType.good && direction == OperationDirection.plus)
                op = Db.Updateable<EResourceInfo>().SetColumns(a => new EResourceInfo() { goodNum = a.goodNum +1 });
            else if (praizeType == PraizeType.bad && direction == OperationDirection.plus)
                op = Db.Updateable<EResourceInfo>().SetColumns(a => new EResourceInfo() { badNum = a.badNum + 1 });
            else if (praizeType == PraizeType.bad && direction == OperationDirection.minus)
                op = Db.Updateable<EResourceInfo>().SetColumns(a => new EResourceInfo() { badNum = a.badNum -1 });

            else if (praizeType == PraizeType.good && direction == OperationDirection.update)
                op = Db.Updateable<EResourceInfo>().SetColumns(a => new EResourceInfo() { goodNum = a.goodNum+1, badNum = a.badNum - 1 });
            else if (praizeType == PraizeType.bad && direction == OperationDirection.update)
                op = Db.Updateable<EResourceInfo>().SetColumns(a => new EResourceInfo() { badNum = a.badNum + 1, goodNum = a.goodNum - 1 });

            op =op.Where(a=>a.Code == resCode);

            return  op.ExecuteCommandHasChangeAsync();
        }

        public Task<bool> UpdateCommentPraized_GoodNum(long commentId, OperationDirection direction)
        {
            var op = Db.Updateable<EComment_Res>().SetColumns(a => new EComment_Res() { goodNum = a.goodNum +1 });
           if (direction == OperationDirection.minus)
                op = Db.Updateable<EComment_Res>().SetColumns(a => new EComment_Res() { goodNum = a.goodNum - 1 });

            op = op.Where(a => a.Id == commentId);

            return op.ExecuteCommandHasChangeAsync();
        }

        public Task<bool> UpdateCommentReplyPraized_GoodNum(long commentReplyId, OperationDirection direction)
        {
            var op = Db.Updateable<ECommentReply_Res>().SetColumns(a => new ECommentReply_Res() { goodNum = a.goodNum + 1 });
            if (direction == OperationDirection.minus)
                op = Db.Updateable<ECommentReply_Res>().SetColumns(a => new ECommentReply_Res() { goodNum = a.goodNum - 1 });

            op = op.Where(a => a.Id == commentReplyId);

            return op.ExecuteCommandHasChangeAsync();
        }

    }
}
