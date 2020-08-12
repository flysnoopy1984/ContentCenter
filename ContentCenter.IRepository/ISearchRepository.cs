using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface ISearchRepository:IBaseRepository<ESearchKeyLog>
    {
        Task<List<SearchSuggetResult>> getSuggest(SearchSuggetReq searchSuggetReq);

    }
}
