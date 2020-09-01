using ContentCenter.IRepository;
using ContentCenter.Model;
using IQB.Util.Models;
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

        public async Task<ModelPager<VueCommentReply>> GetReplysByCommentId(QComment_Reply query)
        {
          ModelPager<VueCommentReply> result = new ModelPager<VueCommentReply>(query.pageIndex, query.pageSize);
          var qOwn = Db.Queryable<EPraize_CommentReply>().Where(c => c.commentId == query.commentId && c.userId == query.reqUserId);
          var q = base.Db.Queryable<ECommentReply_Res, EUserInfo>((c, u) => new object[]
          {
                JoinType.Inner,c.authorId == u.Id
          })
          .Where(c => c.commentId == query.commentId)
          .Select((c, u) => new VueCommentReply
          {
              CreateDateTime = c.CreateDateTime,// c.CreateDateTime.ToString("yyyy-MM-dd hh:MM"),
              authorId = u.Id,
              authorName = u.NickName,
              replyId = c.Id,
              content = c.content,
              goodNum = c.goodNum,
              headerUrl = u.HeaderUrl,
              replyAuthorId = c.replyAuthorId,
              replyAuthorName = c.replyName,
             
          });

        var mainSql = Db.Queryable(q, qOwn, JoinType.Left, (j1, j2) => j1.replyId == j2.replyId)
            .OrderBy(j1 => j1.goodNum, OrderByType.Desc)
            .OrderBy(j1 => j1.CreateDateTime, OrderByType.Desc)
            .Select((j1, j2) => new VueCommentReply
            {
                CreateDateTime = j1.CreateDateTime,
                authorId = j1.authorId,
                authorName = j1.authorName,
                replyId = j1.replyId,
                content = j1.content,
                goodNum = j1.goodNum,
                headerUrl = j1.headerUrl,
                replyAuthorId = j1.replyAuthorId,
                replyAuthorName = j1.replyAuthorName,
                userPraizeType = j2.PraizeType
            });

            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;
            return result;
        }

        public async Task<ModelPager<VueUserCommReply>> queryUserCommReply(QUserCommReply query)
        {
            ModelPager<VueUserCommReply> result = new ModelPager<VueUserCommReply>(query.pageIndex, query.pageSize);
            var mainSql = Db.Queryable<ECommentReply_Res, EComment_Res, EUserInfo, EBookInfo >((r, c,cu,b) => new object[]
            {
               
                JoinType.Inner,r.commentId == c.Id,
                 JoinType.Inner,r.authorId == cu.Id,
                JoinType.Inner,c.parentRefCode == b.Code,

            })
            .Where((r, c, cu, b) => r.authorId == query.userId)
            .OrderBy(r => r.CreateDateTime, OrderByType.Desc)
            .Select((r, c, cu, b) => new VueUserCommReply
            {
                bookCode = b.Code,
                bookName = b.Title,
                bookCoverUrl = b.CoverUrl,
                commentId = c.Id,
                commentAuthorId = c.authorId,
                commentAuthor = cu.NickName,
                pCommentDateTime = c.CreateDateTime,
                commentContent = c.content,
                replyId = r.Id,
                replyContent = r.content,
                replyTarget = r.replyName,
                pReplyDateTime = r.CreateDateTime,

            });

            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;
            return result;
          
        }
    }
}
