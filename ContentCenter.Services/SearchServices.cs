using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using IQB.Util;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Services
{
    public class SearchServices : BaseServices<ESearchKeyLog>, ISearchServices
    {
        private IBookRepository _bookRepository;
        private ISearchRepository _searchRepository;
        public SearchServices(ISearchRepository searchRepository, IBookRepository bookRepository)
         : base(searchRepository)
        {
            _searchRepository = searchRepository;
            _bookRepository = bookRepository;
        }

        public long logSearchKey(ESearchKeyLog searchKeyLog)
        {
            /*暂时不删除收缩记录*/
            //int c = _searchRepository.GetCount(a => a.UserId == searchKeyLog.UserId).Result;
            //if(c>10)
            if (string.IsNullOrEmpty(searchKeyLog.UserId))
            {
                throw new Exception("未知用户无法操作");
            }
            var existObj = _searchRepository.GetByExpSingle(a => a.UserId == searchKeyLog.UserId && a.searchKey == searchKeyLog.searchKey).Result;
            if (existObj != null)
            {
               var r= _searchRepository.UpdatePart_WithObj(existObj, a => new ESearchKeyLog { UpdateDateTime = DateTime.Now }).Result;
               return r ? 1 : 0;
            }
            else
                return _searchRepository.Add(searchKeyLog).Result;

            // existObj.UpdateDateTime = DateTime.Now;
            //if(c>10)


        }

        public ResultPager<RBookList> searchBook(SearchReq searchRequest)
        {
            ResultPager<RBookList> result = new ResultPager<RBookList>();
            if(string.IsNullOrEmpty(searchRequest.userId))
            {
                result.ErrorMsg = "未知用户无法搜索";
                return result;
            }

            if (string.IsNullOrEmpty(searchRequest.keyword.Trim()))
                result.Message = "没有查询内容";
            else
            {
                RefAsync<int> totalNumber = new RefAsync<int>();
                result.PageData.datas = _bookRepository.searchByNameAndAuthor(searchRequest, totalNumber).Result;
                try
                {
                    if (searchRequest.keyword.Length > 100)
                        searchRequest.keyword = searchRequest.keyword.Substring(0, 100);
                    logSearchKey(new ESearchKeyLog
                    {
                        UserId = searchRequest.userId,
                        searchKey = searchRequest.keyword
                    });
                }
                catch(Exception logEx)
                {
                    NLogUtil.cc_ErrorTxt("searchBook: 添加搜索记录出错:" + logEx.Message);
                }
               
            }
            return result;    
        }

        public List<SearchSuggetResult> getSuggest(SearchSuggetReq searchSuggetReq)
        {
            if (string.IsNullOrEmpty(searchSuggetReq.userId))
            {
                throw new Exception("未知用户无法操作");
            }
            return _searchRepository.getSuggest(searchSuggetReq).Result;
            
        }
    }
}
