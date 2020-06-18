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
    public class BookRepository: BaseRepository<EBookInfo>, IBookRepository
    {
        public BookRepository(ISqlSugarClient[] sugarClient) 
            : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.BookDbKey))
        {
          
        }

        public Task<List<RBookSimple>> GetSimpleBookByTag(int pageIndex, int pageSize, string tagCode)
        {
            var q = Db.Queryable<EBookInfo, EBookTag>((b, bt) => new object[]{
                JoinType.Left,b.Code == bt.BookCode,bt.TagCode == tagCode
            });
            var r = GetBookSimpleFromData<EBookTag>(q);
         
            return r.ToPageListAsync(pageIndex, pageSize);
        }

        public Task<List<RBookSimple>> GetSimpleBookBySection(int pageIndex, int pageSize, string secCode)
        {
            //var q = Db.Queryable<EBookInfo, ESection>((b, bt) => new object[]{
            //    JoinType.Left,b.Code == bt.BookCode,bt.TagCode == tagCode
            //});
            //var r = GetBookSimpleFromData<EBookTag>(q);

            //return r.ToPageListAsync(pageIndex, pageSize);
            return null;
        }

        #region  Share Private
        private ISugarQueryable<RBookSimple>  GetBookSimpleFromData<T>(ISugarQueryable<EBookInfo,T> q)
        {
            var r = q.Select((b, bt) => new RBookSimple
            {
                Code = b.Code,
                CoverUrl = b.CoverUrl,
                Name = b.Title,
                Author = b.AuthorCode,
                Score = b.Score.ToString("0.0"),
                Summery = b.Summery

            });
            return r;
        }
        #endregion

       

        public Task<List<RTag>> GetTagList(int number=0, OrderByType orderByType = OrderByType.Desc)
        {
            var q = Db.Queryable<ETag, EBookTag>((tag, bt) => new object[] {
                JoinType.Inner,tag.Code == bt.TagCode
                })
                .GroupBy((tag, bt) => new
                {
                    tag.Code,
                    tag.Name
                })
                .Select((tag, bt) => new RTag
                {
                    Code = tag.Code,
                    Name = tag.Name,
                    Count = SqlFunc.AggregateCount(tag.Code)

                }).MergeTable()
                .OrderBy(t => t.Count, orderByType);

            if (number > 0)
                return q.Take(number).ToListAsync();
            else
                return q.ToListAsync();
           
        }

        public Task<List<ESection>> GetWebSection(SectionType sectionType)
        {
            var q = Db.Queryable<ESection>();
            if (sectionType != SectionType.All)
                q = q.Where(a => a.SectionType == sectionType);
            q = q.OrderBy(a => a.SectionType);
          
            return q.ToListAsync();
        }

    }
}
