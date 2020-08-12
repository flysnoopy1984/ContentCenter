using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model;
using IQB.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SearchController : CCBaseController
    {
        private ISearchServices _searchServices;
        public SearchController(ISearchServices searchServices)
        {
            _searchServices = searchServices;
        }

        [HttpPost]
        public ResultPager<RBookList> BookNameAndAuthor(SearchReq req)
        {
            ResultPager<RBookList> result = null;
            try
            {
                req.userId = this.getUserId();
                result = _searchServices.searchBook(req);
            }
            catch (Exception ex)
            {
                if (result == null) result = new ResultPager<RBookList>();
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultList<SearchSuggetResult> getSuggest(SearchSuggetReq req)
        {
            ResultList<SearchSuggetResult> result = new ResultList<SearchSuggetResult>();
            try
            {
                req.userId = this.getUserId();
                result.List = _searchServices.getSuggest(req);
            }
            catch (Exception ex)
            {
               
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
    }
}