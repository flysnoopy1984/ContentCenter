using ContentCenter.Common;
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
    public class CommentRepository : BaseRepository<EComment_Res>, ICommentRepository
    {
        public CommentRepository(ISqlSugarClient[] sugarClient)
          : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

     

        public async Task<ModelPager<VueCommentInfo>> GetCommentsByResCodes(QComment_Res query)
        {

            ModelPager<VueCommentInfo> result = new ModelPager<VueCommentInfo>(query.pageIndex,query.pageSize);

            var qOwn = Db.Queryable<EPraize_Comment>().Where(c => c.RefCode == query.resCode && c.userId == query.reqUserId);
            var q = base.Db.Queryable<EComment_Res, EUserInfo>((c, u) => new object[]
            {
                JoinType.Inner,c.authorId == u.Id
            })
            .Where(c=>c.refCode == query.resCode)
            .Select((c, u) => new VueCommentInfo
            {
                CreateDateTime = c.CreateDateTime,// c.CreateDateTime.ToString("yyyy-MM-dd hh:MM"),
                authorId = u.Id,
                authorName = u.NickName,
                commentId = c.Id,
                content = c.content,
                goodNum = c.goodNum,
                headerUrl = u.HeaderUrl,
                replyNum = c.replyNum
            });

            var mainSql = Db.Queryable(q, qOwn, JoinType.Left, (j1, j2) => j1.commentId == j2.commentId)
               .OrderBy(j1 => j1.goodNum, OrderByType.Desc)
               .OrderBy(j1 => j1.replyNum, OrderByType.Desc)
               .OrderBy(j1 => j1.CreateDateTime, OrderByType.Desc)
               .Select((j1, j2) => new VueCommentInfo
               {
                   CreateDateTime = j1.CreateDateTime,
                   authorId = j1.authorId,
                   authorName = j1.authorName,
                   commentId = j1.commentId,
                   content = j1.content,
                   goodNum = j1.goodNum,
                   headerUrl = j1.headerUrl,
                   replyNum = j1.replyNum,
                   userPraizeType = j2.PraizeType
               });

            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;
            return result;

        }

        public bool UpdateComment_ReplyNum(long commentId, OperationDirection direction,int num=1)
        {
            var op = Db.Updateable<EComment_Res>().SetColumns(a => new EComment_Res() { replyNum = a.replyNum+ num });
            if (direction == OperationDirection.minus)
                op = Db.Updateable<EComment_Res>().SetColumns(a => new EComment_Res() { replyNum = a.replyNum - num });

            op = op.Where(a => a.Id == commentId);

            return op.ExecuteCommandHasChange();
        }
    }

}
