using ContentCenter.IRepository;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class SearchRepository: BaseRepository<ESearchKeyLog>, ISearchRepository
    {
        public SearchRepository(ISqlSugarClient[] sugarClient)
           : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public Task<List<SearchSuggetResult>> getSuggest(SearchSuggetReq searchSuggetReq)
        {
            var q = Db.Queryable<ESearchKeyLog>()
                    .Where(a => a.UserId == searchSuggetReq.userId)
                    .WhereIF(!string.IsNullOrEmpty(searchSuggetReq.inputWord),a=>a.searchKey.Contains(searchSuggetReq.inputWord))
                    .Take(searchSuggetReq.MaxLine)
                    .OrderBy(a=>a.UpdateDateTime,OrderByType.Desc)
                    .GroupBy(a=>new { a.searchKey,a.UpdateDateTime })
                    .Select(a => new SearchSuggetResult
                    {
                        value = a.searchKey
                    });
        
            var r = q.ToListAsync();
           
            return r;
                  

        }
    }
}
