using ContentCenter.Common;
using ContentCenter.IRepository;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using ContentCenter.Model.Products.Res;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class ResourceRepository : BaseRepository<EResourceInfo>, IResourceReponsitory
    {
        public ResourceRepository(ISqlSugarClient[] sugarClient)
          : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public Task<int> SameResCount(string userId,string refCode, ResType resType, string fileType, bool includeDelete = false)
        {
            var exp = Expressionable.Create<EResourceInfo>()
                .And(a => a.RefCode == refCode)
                .And(a => a.ResType == resType)
                .And(a => a.FileType == fileType)
                .And(a=>a.Owner == userId)
                .AndIF(!includeDelete, a => a.IsDelete == false).ToExpression();

            return base.GetCount(exp);

        }

        public bool LogicDelete(string resCode)
        {
            return base.UpdatePart_NoObj_Sync(a => new EResourceInfo() { IsDelete = true }, a => a.Code == resCode);
        }

      
        // praizeUserId 当前用户对于此资源是否点赞，
        // includeDeleteRes 查询是否包含删除的资源
        // fixedResCode 固定的资源（页面置顶）
        private ISugarQueryable<VueResInfo> sqlResByCode(
            string bookCode,
            string praizeUserId,
            string fixedResCode=null
            )
        {
            bool hasFiexedResCode = !string.IsNullOrEmpty(fixedResCode);
            var orderByPraize = $"case when j1.goodNum <{ SysRules.OrderByPraizeStartNum} then 0 else j1.goodNum end desc";

            var qPraize_User = Db.Queryable<EPraize_Res>()
              .Where(p => p.userId == praizeUserId && bookCode == p.bookCode);

            var q = base.Db.Queryable<EResourceInfo, EUserInfo>((a, u) => new object[]
             {
                JoinType.Inner,a.Owner == u.Id
             })
            .Where(a => a.RefCode == bookCode && a.IsDelete == false)
            .Select((a, u) => new VueResInfo
            {
                CreateDateTime = a.CreateDateTime,
                fileType = a.FileType,
                ownerId = u.Id,
                ownerName = u.NickName,
                resType = (int)a.ResType,
                goodNum = a.goodNum,
                badNum = a.badNum,
                resName = a.ResType == ResType.BookOss?a.OrigFileName:a.Url,
                resCode = a.Code,
                resId = a.Id, //用于消息 20201015
                bookCode = a.RefCode
            });


            var mainSql = Db.Queryable(q, qPraize_User, JoinType.Left, (j1, j2) => j1.resCode == j2.ResCode)
                .OrderByIF(hasFiexedResCode,
                //如果是固定资源，query时通过排序置顶 20201027
                $"case when j1.resCode = '{fixedResCode}' then '' else  j1.resCode end") 
                .OrderBy(orderByPraize)
                .OrderBy(j1 => j1.CreateDateTime, OrderByType.Desc)
                .Select((j1, j2) => new VueResInfo
                {
                    CreateDateTime = j1.CreateDateTime,
                    fileType = j1.fileType,
                    ownerId = j1.ownerId,
                    ownerName = j1.ownerName,
                    resType = j1.resType,
                    goodNum = j1.goodNum,
                    resName = j1.resName,
                    badNum = j1.badNum,
                    resCode = j1.resCode,
                    resId = j1.resId, //用于消息 20201015
                    userPraizeType = j2.PraizeType,
                    bookCode = j1.bookCode,
                  
                });
          
            return mainSql;
        }

        public async Task<ModelPager<VueResInfo>> GetResByRefCode(QRes qRes)
        {
            ModelPager<VueResInfo> result = new ModelPager<VueResInfo>(qRes.pager.pageIndex, qRes.pager.pageSize);

         
            var mainSql = this.sqlResByCode(qRes.refCode, qRes.reqUserId, qRes.fixedResCode);
            RefAsync<int> totalNumber = new RefAsync<int>();

            result.datas = await mainSql.ToPageListAsync(qRes.pager.pageIndex, qRes.pager.pageSize, totalNumber);
            result.totalCount = totalNumber;
            return result;

        }

        public int logRequireRes(string resCode, string requireUserId)
        {
            var op = base.Db.Insertable(new EResourceRequire_Log
            {
                requireUserId = requireUserId,
                resCode = resCode,
                requireDateTime = DateTime.Now,
            });
            return op.ExecuteCommand();
        }

        public bool addRequireResNum(string resCode)
        {
            var op = Db.Updateable<EResourceInfo>().SetColumns(a => new EResourceInfo() { requireNum = a.requireNum + 1 });
            op = op.Where(a => a.Code == resCode);
            var r = op.ExecuteCommandHasChange();
            return r;
            // throw new NotImplementedException();
        }


        
        public ModelPager<VueUserRes> queryUserRes_GroupByBook(QUserRes query)
        {
            ModelPager<VueUserRes> result = new ModelPager<VueUserRes>(query.pageIndex, query.pageSize);
            var mainSql = Db.Queryable<EResourceInfo, EBookInfo>((r, b) => new object[]
            {
                JoinType.Inner,r.RefCode == b.Code
            })
            .Where((r, b) => r.Owner == query.userId && r.IsDelete == false)
            .GroupBy((r, b) => new { b.Code, r.Owner, b.Title, b.CoverUrl })
            .Select((r, b) => new VueUserRes
            {
                bookCode = b.Code,
                bookCoverUrl = b.CoverUrl,
                bookName = b.Title,
            });

            // RefAsync<int> totalNumber = new RefAsync<int>();
            int totalNumber =0;
            result.datas =  mainSql.ToPageList(query.pageIndex, query.pageSize, ref totalNumber);
            result.totalCount = totalNumber;
            result.datas = arrangeUserRes(result.datas);
            return result;
        }

        // 获取书本资源列表，再次获取书本对应的资源。整理数据
        private List<VueUserRes> arrangeUserRes(List<VueUserRes> list)
        {
            if (list.Count == 0) return list;
            
            List<VueBookRes> result = new List<VueBookRes>();
          
            var exp = Expressionable.Create<EResourceInfo>();
            foreach(var param in list)
            {
                exp = exp.Or(a => a.RefCode == param.bookCode);
            }

            var mainSql = Db.Queryable<EResourceInfo>().Where(exp.ToExpression())
              //  .WhereIF(!string.IsNullOrEmpty(query.fixedResCode), a => a.Code != query.fixedResCode)
                .Select(a => new VueBookRes
                {
                    CreateDateTime = a.UpdateDateTime,
                    bookCode = a.RefCode,
                    fileType = a.FileType,
                    origFileName = a.OrigFileName,
                    resType = a.ResType,
                    resCode = a.Code,
                    url = a.Url,
                });
               
            List<VueBookRes> vbList = mainSql.ToList();
            foreach(var ur in list)
            {
                ur.resList = vbList.Where(a => a.bookCode == ur.bookCode)
                    .OrderByDescending(a=>a.CreateDateTime)
                    .ToList();
            }
            return list;
        }

        public EUserInfo getResoureOwnerId(string resCode)
        {
           // var r = Db.Queryable<EResourceInfo>().Where(a => a.Code == resCode)
            var q = Db.Queryable<EResourceInfo, EUserInfo>((r, u) => new object[]{
                JoinType.Inner,r.Owner ==u.Id
            })
            .Where((r, u) => r.Code == resCode)
            .Select((r,u)=>new EUserInfo
            {
                Id = u.Id,
                NickName = u.NickName,
                HeaderUrl = u.HeaderUrl
            });

            return q.First();
          
        }

        public ResSimple getSimpleByCommentId(long commentId)
        {
            var r = Db.Queryable<EComment_Res,EResourceInfo>((c, r) => new object[]{
                JoinType.Inner,c.refCode == r.Code
            }).Where((c,r) => c.Id == commentId)
            .Select((c, r) => new ResSimple
            {
                Code = r.Code,
                ResName = r.ResType == ResType.BookOss ? r.OrigFileName : r.Url,
                ResType = r.ResType
            });
            return r.First();
        }

        public ResSimple getSimpleByCode(string resCoe)
        {
            var r = Db.Queryable<EResourceInfo>().Where(c => c.Code == resCoe)
                 .Select(a => new ResSimple
                 {
                     Code = a.Code,
                     ResName = a.ResType == ResType.BookOss ? a.OrigFileName : a.Url,
                     ResType = a.ResType
                    //Name = a.ResType == ResType.BookOss ? $"[文件]-{a.OrigFileName}" : $"[URL]-{a.Url}"
                }) ;
            return r.First();
        }
    }
}
