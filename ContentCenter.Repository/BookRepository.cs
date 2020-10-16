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
            var r = getSelectBookList(q);
            //var r = q.Select((b, bt) => new RBookList
            //{
            //    Code = b.Code,
            //    CoverUrl = b.CoverUrl,
            //    Name = b.Title,
            //    Author = b.AuthorCode,
            //    Score = b.Score.ToString(),
            //    Summery = b.Summery.Substring(0, 100),
            //    ResourceCount = b.ResoureNum,

            //});

            return r.ToPageListAsync(pageIndex, pageSize, totalNumber);
        }

        public Task<List<RBookList>> GetBookListBySection_ST(int pageIndex, int pageSize, string secCode, RefAsync<int> totalNumber)
        {
            var q = Db.Queryable<EBookInfo, EBookTag,ESectionTag>((b, bt,st) => new object[]{
                JoinType.Inner,b.Code == bt.BookCode,
                JoinType.Inner,bt.TagCode == st.TagCode
            });
            q = q.Where((b, bt, st) =>  st.SectionCode== secCode);
            var r = getSelectBookList(q);
            //var r = q.Select((b,bt,st)=> new RBookList{
            //    Code = b.Code,
            //    CoverUrl = b.CoverUrl,
            //    Name = b.Title,
            //    Author = b.AuthorCode,
            //    Score = b.Score.ToString(),
            //    Summery = b.Summery.Substring(0, 100),
            //    ResourceCount = b.ResoureNum,
            //});

            return r.ToPageListAsync(pageIndex, pageSize, totalNumber);
           
        }

        public Task<List<RBookList>> GetBookListBySection_DT(int pageIndex, int pageSize, string secCode, RefAsync<int> totalNumber)
        {
            var q = Db.Queryable<EBookInfo, ESectionBook>((b, sb) => new object[]{
                JoinType.Inner,b.Code == sb.BookCode,
               
            });
            q = q.Where((b, sb) => sb.SectionCode == secCode);
            q = q.OrderBy((b, sb) => b.CreateDateTime,OrderByType.Desc);
            var r = getSelectBookList(q);
            //var r = q.Select((b,sb) => new RBookList
            //{
            //    Code = b.Code,
            //    CoverUrl = b.CoverUrl,
            //    Name = b.Title,
            //    Author = b.AuthorCode,
            //    Score = b.Score.ToString(),
            //    Summery = b.Summery.Substring(0, 100),
            //    ResourceCount = b.ResoureNum,
            //});

            return r.ToPageListAsync(pageIndex, pageSize, totalNumber);
        }

        public Task<List<RBookList>> GetBookListBySection_HighScroe(int pageIndex, int pageSize, RefAsync<int> totalNumber, int defaultTop)
        {
            var q = Db.Queryable<EBookInfo>().OrderBy(a => a.Score, OrderByType.Desc);
            var r = getSelectBookList(q);
            //var r = q.Select(b => new RBookList
            //{
            //    Code = b.Code,
            //    CoverUrl = b.CoverUrl,
            //    Name = b.Title,
            //    Author = b.AuthorCode,
            //    Score = b.Score.ToString(),
            //    Summery = b.Summery.Substring(0,100),
            //    ResourceCount = b.ResoureNum,
            //});

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
            q = q.OrderBy(a => a.seq, OrderByType.Desc);
          
            return q.ToListAsync();
        }

        public Task<string> TestSql()
        {
            return null;
            //Db.Ado.GetDataTable("")
            //return "";
        }
        public Task<List<RBookList>> searchByNameAndAuthor(SearchReq searchRequest, RefAsync<int> totalNumber)
        {

            var q = Db.Queryable<EBookInfo>();
            q = q.Where(b => b.Title.Contains(searchRequest.keyword) || b.AuthorCode.Contains(searchRequest.keyword))
                .OrderBy(b => b.CreateDateTime, OrderByType.Desc);
            var r = getSelectBookList(q);
            //var r = q.Select((b) => new RBookList
            //{
            //    Code = b.Code,
            //    CoverUrl = b.CoverUrl,
            //    Name = b.Title,
            //    Author = b.AuthorCode,
            //    Score = b.Score.ToString(),
            //    Summery = b.Summery.Substring(0, 100),
            //    ResourceCount = b.ResoureNum,
            //});
         
            return r.ToPageListAsync(searchRequest.pageIndex, searchRequest.pageSize, totalNumber);
          
        }

        public bool UpdateBookResNum(string bookCode, OperationDirection direction)
        {
            var op = Db.Updateable<EBookInfo>().SetColumns(a => new EBookInfo() { ResoureNum = a.ResoureNum + 1,UpdateDateTime=DateTime.Now });
            if(direction == OperationDirection.minus)
                op = Db.Updateable<EBookInfo>().SetColumns(a => new EBookInfo() { ResoureNum = a.ResoureNum + 1, UpdateDateTime = DateTime.Now });

            op = op.Where(a => a.Code == bookCode);

            return op.ExecuteCommandHasChange();
        }

        /// <summary>
        /// 获取有资源文件的书列表
        /// </summary>
        public Task<List<RBookList>> GetBookListBySection_Resource(int pageIndex, int pageSize, RefAsync<int> totalNumber)
        {
            var q = Db.Queryable<EBookInfo>()
                .Where(b => b.ResoureNum > 0)
                .OrderBy(b => b.UpdateDateTime, OrderByType.Desc);

            var r = getSelectBookList(q);
            //var r = q.Select(b => new RBookList
            //{
            //    Code = b.Code,
            //    CoverUrl = b.CoverUrl,
            //    Name = b.Title,
            //    Author = b.AuthorCode,
            //    Score = b.Score.ToString(),
            //    Summery = b.Summery.Substring(0, 100),
            //    ResourceCount = b.ResoureNum,
            //});

            return r.ToPageListAsync(pageIndex, pageSize, totalNumber);
        }
        private ISugarQueryable<RBookList> getSelectBookList(ISugarQueryable<EBookInfo> q)
        {
            ISugarQueryable<RBookList> r;
            r = q.Select(b => new RBookList
            {
                Code = b.Code,
                CoverUrl = b.CoverUrl,
                Name = b.Title,
                Author = b.AuthorCode,
                Score = b.Score.ToString(),
                Summery = b.Summery.Substring(0, 100),
                ResourceCount = b.ResoureNum,
            });

            return r;
        }

        public EBookInfo getBookSimple_ByCode(string bookCode)
        {
            var r =Db.Queryable<EBookInfo>()
                .Where(b => b.Code == bookCode)
                .Select(b => new EBookInfo
                {
                    Code = b.Code,
                    Title = b.Title,
                    CoverUrl = b.CoverUrl,
                });
            return r.First();

            
        }
    }
}
