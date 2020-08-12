using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IServices
{
    public interface ISearchServices: IBaseServices<ESearchKeyLog>
    {
        ResultPager<RBookList> searchBook(SearchReq searchRequest);

        List<SearchSuggetResult> getSuggest(SearchSuggetReq searchSuggetReq);

        long logSearchKey(ESearchKeyLog searchKeyLog);

    }
}
