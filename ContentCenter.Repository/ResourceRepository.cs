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


        public async Task<ModelPager<VueResInfo>> GetResByRefCode(QRes qRes)
        {
            ModelPager<VueResInfo> result = new ModelPager<VueResInfo>(qRes.pager.pageIndex, qRes.pager.pageSize);

            var qPraize_User = Db.Queryable<EPraize_Res>()
                .Where(p => p.userId == qRes.reqUserId && qRes.refCode == p.RefCode);


            var q = base.Db.Queryable<EResourceInfo, EUserInfo>((a, u) => new object[]
             {
                JoinType.Inner,a.Owner == u.Id
             })
            .Where(a => a.RefCode == qRes.refCode)
            .WhereIF(!qRes.includeDelete, a => a.IsDelete == false)
            .Select((a, u) => new VueResInfo
            {
                CreateDateTime = a.CreateDateTime,
                fileType = a.FileType,
                ownerId = u.Id,
                ownerName = u.NickName,
                resType = (int)a.ResType,
                goodNum = a.goodNum,
                badNum = a.badNum,
                resCode = a.Code,
            });


            var mainSql = Db.Queryable(q, qPraize_User, JoinType.Left, (j1, j2) => j1.resCode == j2.ResCode)
                .OrderBy(j1 => j1.goodNum, OrderByType.Desc)
                .OrderBy(j1 => j1.CreateDateTime, OrderByType.Desc)
                .Select((j1, j2) => new VueResInfo
                {
                    CreateDateTime = j1.CreateDateTime,
                    fileType = j1.fileType,
                    ownerId = j1.ownerId,
                    ownerName = j1.ownerName,
                    resType = j1.resType,
                    goodNum = j1.goodNum,
                    badNum = j1.badNum,
                    resCode = j1.resCode,
                    userPraizeType = j2.PraizeType
                });

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

      

        public async Task<ModelPager<VueUserRes>> queryUserRes_GroupByBook(QUserRes query)
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

            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;
            result.datas = arrangeUserRes(result.datas);
            return result;
        }

        /// <summary>
        /// 获取书本资源列表，再次获取书本对应的资源。整理数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
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
                .OrderBy(a=>a.RefCode)
                .Select(a=>new VueBookRes
                {
                    CreateDateTime = a.UpdateDateTime,
                    bookCode = a.RefCode,
                    fileType = a.FileType,
                    origFileName =a.OrigFileName,
                    resType =a.ResType,
                    url = a.Url,
                });
            List<VueBookRes> vbList = mainSql.ToList();
            foreach(var ur in list)
            {
                ur.resList = vbList.Where(a => a.bookCode == ur.bookCode).ToList();
            }



            return list;
        }
    }
}
