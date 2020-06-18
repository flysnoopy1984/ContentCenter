using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
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

        [HttpGet]
        public ResultPager<RBookSimple> getSimplePager(QBookList query)
        {
            ResultPager<RBookSimple> result = new ResultPager<RBookSimple>();
            try
            {
                result.PageData.pageIndex = query.pageIndex;
                result.PageData.pageSize = query.pageSize;
                result.PageData.datas = _bookServices.GetSimpleBookPager(query);
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
            ResultNormal r = new ResultNormal();
            r.Message = "获取Token.Pass Auth";
            return r;
        }

        [HttpGet]
        [Authorize]
        public ResultEntity<EBookInfo> Get(string code)
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
        public ResultEntity<Dictionary<SectionType,List<ESection>>> GetSection(SectionType sectionType=  SectionType.All)
        {
            ResultEntity<Dictionary<SectionType, List<ESection>>> result = new ResultEntity<Dictionary<SectionType, List<ESection>>>();

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