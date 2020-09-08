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
    public class UserBookRepository:BaseRepository<EUserBook>, IUserBookRepository
    {
        public UserBookRepository(ISqlSugarClient[] sugarClient)
           : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public Task<long> AddUserBook(string bookCode, string userId)
        {
            return base.Add(new EUserBook
            {
                createdDateTime = DateTime.Now,
                bookCode = bookCode,
                userId = userId
            });
        }

        public Task<bool> DelUserBook(string bookCode, string userId)
        {
            return base.DeleteRangeByExp(b => b.userId == userId && b.bookCode == bookCode);
        }

        public Task<int> HasFavBook(string bookCode, string userId)
        {
            return base.GetCount(a => a.bookCode == bookCode && a.userId == userId);
        }

        public async Task<ModelPager<VueUserBook>> queryUserBook(QUserBook query)
        {
            ModelPager<VueUserBook> result = new ModelPager<VueUserBook>(query.pageIndex, query.pageSize);
            var mainSql = Db.Queryable<EUserBook, EBookInfo>((ub, b) => new object[]
            {
                JoinType.Inner,ub.bookCode == b.Code
            })
            .Where(ub => ub.userId == query.userId)
            .OrderBy(ub=>ub.createdDateTime,OrderByType.Desc)
            .Select((ub, b) => new VueUserBook
            {
                id = ub.Id,
                Code = b.Code,
                CoverUrl = b.CoverUrl,
                Name = b.Title,
                ResourceCount = b.ResoureNum,
                Score = b.Score,
                CreateDateTime = ub.createdDateTime,
            });
            RefAsync<int> totalNumber = new RefAsync<int>();
            result.datas = await mainSql.ToPageListAsync(query.pageIndex, query.pageSize, totalNumber);
            result.totalCount = totalNumber;
            return result;
        }
    }
}
