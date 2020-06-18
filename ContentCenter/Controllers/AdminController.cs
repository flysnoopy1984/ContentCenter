using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AdminController : CCBaseController
    {
        private IAdminServices _adminServices;
        private IBookServices _bookServices;
        public AdminController(IAdminServices adminServices, IBookServices bookServices)
        {
            _adminServices = adminServices;
            _bookServices = bookServices;
        }

        /// <summary>
        /// 获取Section 和Tag关系
        /// </summary>
        /// <param name="secCode"></param>
        /// <param name="tagCount"></param>
        /// <returns></returns>
        [HttpGet]
        public ResultEntity<RCSectionTagRelation> GetSectionTagRelation(string secCode,int tagCount)
        {
            ResultEntity<RCSectionTagRelation> result = new ResultEntity<RCSectionTagRelation>();
            try
            {
                result.Entity = _adminServices.GetSectionTagRelation(secCode, tagCount);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;

        }
        [HttpPost]
        public ResultNormal SaveSectionTagRelation(InSectionTagList inData)
        {
            ResultNormal result = new ResultNormal();
            try
            {
               var section =  _bookServices.GetSection(inData.secCode);
                if (section != null)
                    _adminServices.SaveSectionTag(section, inData.tagList);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
    }
}