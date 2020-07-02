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
            : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {
          
        }

        public Task<List<RBookList>> GetBookListByTag(int pageIndex, int pageSize, string tagCode, RefAsync<int> totalNumber)
        {
            //= new RefAsync<int>();
            var q = Db.Queryable<EBookInfo, EBookTag>((b, bt) => new object[]{
                JoinType.Inner,b.Code == bt.BookCode
            });
            q = q.Where((b, bt) => bt.TagCode == tagCode);
            var r = q.Select((b, bt) => new RBookList
            {
                Code = b.Code,
                CoverUrl = b.CoverUrl,
                Name = b.Title,
                Author = b.AuthorCode,
                Score = b.Score.ToString(),
                Summery = b.Summery.Substring(0, 100),

            });

            return r.ToPageListAsync(pageIndex, pageSize, totalNumber);
        }

        public Task<List<RBookList>> GetBookListBySection_ST(int pageIndex, int pageSize, string secCode, RefAsync<int> totalNumber)
        {
            var q = Db.Queryable<EBookInfo, EBookTag,ESectionTag>((b, bt,st) => new object[]{
                JoinType.Inner,b.Code == bt.BookCode,
                JoinType.Inner,bt.TagCode == st.TagCode
            });
            q = q.Where((b, bt, st) =>  st.SectionCode== secCode);
            var r = q.Select((b,bt,st)=> new RBookList{
                Code = b.Code,
                CoverUrl = b.CoverUrl,
                Name = b.Title,
                Author = b.AuthorCode,
                Score = b.Score.ToString(),
                Summery = b.Summery.Substring(0, 100),
            });

            return r.ToPageListAsync(pageIndex, pageSize, totalNumber);
           
        }

        public Task<List<RBookList>> GetBookListBySection_DT(int pageIndex, int pageSize, string secCode, RefAsync<int> totalNumber)
        {
            var q = Db.Queryable<EBookInfo, ESectionBook>((b, sb) => new object[]{
                JoinType.Inner,b.Code == sb.BookCode,
               
            });
            q = q.Where((b, sb) => sb.SectionCode == secCode);
            var r = q.Select((b,sb) => new RBookList
            {
                Code = b.Code,
                CoverUrl = b.CoverUrl,
                Name = b.Title,
                Author = b.AuthorCode,
                Score = b.Score.ToString(),
                Summery = b.Summery.Substring(0, 100),
            });

            return r.ToPageListAsync(pageIndex, pageSize, totalNumber);
        }

        public Task<List<RBookList>> GetBookListBySection_HighScroe(int pageIndex, int pageSize, RefAsync<int> totalNumber, int defaultTop)
        {
            var q = Db.Queryable<EBookInfo>().OrderBy(a => a.Score, OrderByType.Desc);
            var r = q.Select(b => new RBookList
            {
                Code = b.Code,
                CoverUrl = b.CoverUrl,
                Name = b.Title,
                Author = b.AuthorCode,
                Score = b.Score.ToString(),
                Summery = b.Summery.Substring(0,100),
            });

            return r.Take(defaultTop).ToPageListAsync(pageIndex, pageSize, totalNumber);
          
        }



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
            q = q.Where(a => a.IsDelete == false);
            if (sectionType != SectionType.All)
                q = q.Where(a => a.SectionType == sectionType);
            q = q.OrderBy(a => a.SectionType);
          
            return q.ToListAsync();
        }

        public Task<string> TestSql()
        {
            return null;
            //Db.Ado.GetDataTable("")
            //return "";
        }

    }
}
