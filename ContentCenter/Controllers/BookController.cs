using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace ContentCenter.Controllers
{
   
    [Route("[controller]/[action]")]
    [ApiController]
    public class BookController : CCBaseController
    {
        private  IBookServices _bookServices;
        public BookController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }

        [HttpPost]
        public ResultPager<RBookList> getBookListPager(QBookList query)
        {
            ResultPager<RBookList> result = new ResultPager<RBookList>();
            try
            {
               
                var pd = _bookServices.GetBookListPager(query);
                result.PageData = pd;
                result.PageData.pageIndex = query.pageIndex;
                result.PageData.pageSize = query.pageSize;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
            
        }

        [HttpGet]
        [Authorize]
        public ResultNormal getNoAuth()
        {
            var a = HttpContext.User;
            ResultNormal r = new ResultNormal();
            r.Message = "获取Token.Pass Auth";
            return r;
        }

        [HttpGet]
        [Authorize]
        public ResultEntity<EBookInfo> Info(string code)
        {
            ResultEntity<EBookInfo> result = new ResultEntity<EBookInfo>();
            try
            {
                result.Entity = _bookServices.Info(code);
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;

        }

        /// <summary>
        /// 获取网站的Section
        /// </summary>
        /// <param name="sectionType"></param>
        /// <returns></returns>
        [HttpGet]
        public ResultEntity<Dictionary<string,List<ESection>>> GetSection(SectionType sectionType=  SectionType.All)
        {
            ResultEntity<Dictionary<string, List<ESection>>> result = new ResultEntity<Dictionary<string, List<ESection>>>();

            try
            {
                result.Entity = _bookServices.GetWebSection(sectionType);
            }
            catch (Exception ex)
            {
                result.ErrorMsg =$"GetSection{ ex.Message}";
            }
            return result;
        }

        /// <summary>
        /// 获取Tag列表
        /// </summary>
        /// <param name="number"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        [HttpGet]
        public ResultList<RTag> GetTags(int number,OrderByType orderByType)
        {
            ResultList<RTag> result = new ResultList<RTag>();

            try
            {
                result.List = _bookServices.GetTagList(number, orderByType);
               // result.Entity = _bookServices.GetWebSection(sectionType);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = $"GetTags{ ex.Message}";
            }
            return result;
        }
       
    }
}