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
    public class ResourceRepository:BaseRepository<EResourceInfo>, IResourceReponsitory
    {
        public ResourceRepository(ISqlSugarClient[] sugarClient)
          : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public Task<int> SameResCount(string refCode, ResType resType, string fileType, bool includeDelete = false)
        {
            var exp = Expressionable.Create<EResourceInfo>()
                .And(a => a.RefCode == refCode)
                .And(a => a.ResType == resType)
                .And(a => a.FileType == fileType)
                .AndIF(!includeDelete, a => a.IsDelete == false).ToExpression();

            return base.GetCount(exp);
         
        }

        public Task<bool> LogicDelete(string resCode)
        {
            return base.UpdatePart_NoObj(a => new EResourceInfo() { IsDelete = true }, a => a.Code == resCode);
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
            
            result.datas =await mainSql.ToPageListAsync(qRes.pager.pageIndex, qRes.pager.pageSize, totalNumber);
            result.totalCount = totalNumber;
            return result;

        }
    }
}
