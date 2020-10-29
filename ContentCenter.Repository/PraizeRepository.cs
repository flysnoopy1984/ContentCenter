using ContentCenter.IRepository;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
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
     
        public long AddPraize_Comment(EPraize_Comment praize)
        {
            var insertable = Db.Insertable(praize);
            return insertable.ExecuteReturnBigIdentity();
        }


        public long AddPraize_CommentReply(EPraize_CommentReply praize)
        {
            var insertable = Db.Insertable(praize);
            return insertable.ExecuteReturnBigIdentity();
        }

        public Task<int> HasPraized_Comment_Res(long commentId, string userId)
        {
            return Db.Queryable<EPraize_Comment>().Where(a => a.commentId== commentId && a.userId == userId). CountAsync();
        }

        public Task<int> HasPraized_CommentReply_Res(long commentReplyId, string userId)
        {
            return Db.Queryable<EPraize_CommentReply>().Where(a => a.replyId == commentReplyId && a.userId == userId).CountAsync();
        }

        public Task<int> HasPraized_Res(string resCode, string userId)
        {
            return base.GetCount(a => a.ResCode == resCode && a.userId == userId);
        }
        public bool UpdatePraized_Res(PraizeType praizeType, string resCode, string userId)
        {
           

            var op = Db.Updateable<EPraize_Res>().SetColumns(a => new EPraize_Res() { PraizeType = praizeType });
            op = op.Where(a => a.ResCode == resCode && a.userId == userId);

            return op.ExecuteCommandHasChange();
        }

        public bool DeletePraized_Res(string resCode, string userId)
        {
        
            return base.DeleteRangeByExp_Sync(a => a.ResCode == resCode && a.userId == userId);
        }

        public Task<EPraize_Res> GetPraize_Res(string resCode, string userId)
        {
            return GetByExpSingle(a => a.userId == userId && a.ResCode == resCode);
        }

        public bool DeletePraized_Comment_Res(long commentId, string userId)
        {
            var exp = Expressionable.Create<EPraize_Comment>()
             .And(a => a.commentId == commentId)
             .AndIF(!string.IsNullOrEmpty(userId), a => a.userId == userId).ToExpression();

            var op = Db.Deleteable(exp);
            return  op.ExecuteCommandHasChange();
        }

        public int DeletePraized_CommentReply_Res(long replyId, string userId)
        {
            var exp = Expressionable.Create<EPraize_CommentReply>()
            .And(a => a.replyId == replyId)
            .AndIF(!string.IsNullOrEmpty(userId), a => a.userId == userId).ToExpression();

            var op = Db.Deleteable(exp);
            return op.ExecuteCommand();
        }

        public bool UpdateResPraizedNum(string resCode, PraizeType praizeType, OperationDirection direction)
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

            return  op.ExecuteCommandHasChange();
        }

        public bool UpdateCommentPraized_GoodNum(long commentId, OperationDirection direction, int num = 1)
        {
            var op = Db.Updateable<EComment_Res>().SetColumns(a => new EComment_Res() { goodNum = a.goodNum + num });
           if (direction == OperationDirection.minus)
                op = Db.Updateable<EComment_Res>().SetColumns(a => new EComment_Res() { goodNum = a.goodNum - num });

            op = op.Where(a => a.Id == commentId);
          
            return op.ExecuteCommandHasChange();
        }

        public bool UpdateCommentReplyPraized_GoodNum(long commentReplyId, OperationDirection direction,int num=1)
        {
            var op = Db.Updateable<ECommentReply_Res>().SetColumns(a => new ECommentReply_Res() { goodNum = a.goodNum + num });
            if (direction == OperationDirection.minus)
                op = Db.Updateable<ECommentReply_Res>().SetColumns(a => new ECommentReply_Res() { goodNum = a.goodNum - num });

            op = op.Where(a => a.Id == commentReplyId);

            return op.ExecuteCommandHasChange();
        }

        public int DeletePraized_AllReplyBelowComment(long commentId)
        {
            var op = Db.Deleteable<EPraize_CommentReply>(a => a.commentId == commentId);
            return op.ExecuteCommand();
        }

        public async Task<ModelPager<VueUserPraize>> queryUserResPraize(QUserPraize query)
        {
            ModelPager<VueUserPraize> result = new ModelPager<VueUserPraize>(query.pageIndex, query.pageSize);

            var prSql = sql_UserResPraize(query);
            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await prSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;

            return result;
        }

        public async Task<ModelPager<VueUserPraize>> queryUserCommentPraize(QUserPraize query)
        {
            ModelPager<VueUserPraize> result = new ModelPager<VueUserPraize>(query.pageIndex, query.pageSize);

            var prSql = sql_UserCommentPraize(query);
            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await prSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;

            return result;
        }

        public async Task<ModelPager<VueUserPraize>> queryUserCommentReplyPraize(QUserPraize query)
        {
            ModelPager<VueUserPraize> result = new ModelPager<VueUserPraize>(query.pageIndex, query.pageSize);

            var prSql = sql_UserCommReplyPraize(query);
            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await prSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;

            return result;
        }

        #region Sql语句
        /// <summary>
        /// 回复的点赞
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private ISugarQueryable<VueUserPraize> sql_UserCommReplyPraize(QUserPraize query)
        {
            ISugarQueryable<VueUserPraize> prSql = Db.Queryable<EPraize_CommentReply, ECommentReply_Res, EComment_Res,EBookInfo >((p, r, c,b) => new object[]
              {
                  JoinType.Inner,p.replyId == r.Id,
                  JoinType.Inner,p.commentId == c.Id,
                  JoinType.Inner,c.parentRefCode == b.Code
              })
            .Where(p => p.userId == query.userId)
            .OrderBy(p => p.praizeDate, OrderByType.Desc)
            .Select((p, r, c, b) => new VueUserPraize
            {
                id = p.Id,
                CreateDateTime = p.praizeDate,
                resCode = c.refCode,
                commentId = c.Id,
                code = r.Id.ToString(),
                bookCode = b.Code,
                bookName = b.Title,
                content =  c.content,
                bookUrl = b.CoverUrl,
                refContent = r.content
            });
            return prSql;
        }

        /// <summary>
        /// 评论获得的赞
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private ISugarQueryable<VueUserPraize> sql_UserCommentPraize(QUserPraize query)
        {
            ISugarQueryable<VueUserPraize> prSql = Db.Queryable<EPraize_Comment,EComment_Res, EBookInfo>((p, c, b) => new object[]
             {

                JoinType.Inner,p.commentId == c.Id,
                JoinType.Inner,c.parentRefCode == b.Code

             })
            .Where(p => p.userId == query.userId)
            .OrderBy(p => p.praizeDate, OrderByType.Desc)
            .Select((p, c, b) => new VueUserPraize
            {
                id = p.Id,
                CreateDateTime = p.praizeDate,
                code = c.Id.ToString(),
                resCode = c.refCode,
                commentId = c.Id,
                bookCode = b.Code,
                bookName = b.Title,
                bookUrl = b.CoverUrl,
                content = c.content,
            });
           
        
            return prSql;
        }
        /// <summary>
        /// 资源获得的赞
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private ISugarQueryable<VueUserPraize> sql_UserResPraize(QUserPraize query)
        {
            ISugarQueryable<VueUserPraize> prSql = Db.Queryable<EPraize_Res, EResourceInfo, EBookInfo>((p, r, b) => new object[]
             {

                JoinType.Inner,p.ResCode == r.Code,
                JoinType.Inner,p.bookCode == b.Code

             })
            .Where(p => p.userId == query.userId)
            .OrderBy(p => p.praizeDate, OrderByType.Desc)
            .Select((p, r, b) => new VueUserPraize
            {
                id = p.Id,
                CreateDateTime = p.praizeDate,
                code = r.Code,
                resCode = r.Code,
                bookCode = b.Code,
                bookName = b.Title,
                bookUrl = b.CoverUrl,
                resPraizeType = p.PraizeType,
                content = b.Title + "的" + "[" + r.FileType + "资源文件]-" + (r.ResType == ResType.BookOss ? r.OrigFileName : "url地址"),
            });
           
            return prSql;
        }

        #endregion
    }
}
