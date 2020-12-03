using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Services
{
    public class AdminServices : BaseServices<ESectionTag>, IAdminServices
    {
        private IAdminRepository _adminDb;
        private IBookRepository _bookDb;
       

        public AdminServices(IAdminRepository adminRepository, IBookRepository bookRepository)
         : base(adminRepository)
        {
            _bookDb = bookRepository;
            _adminDb = adminRepository;
        }

       

        /// <summary>
        /// 获取Section下有多少个Tag和多少个未使用的Tag
        /// </summary>
        /// <param name="tagNumber">获取Tag的数量</param>
        /// <returns></returns>
        public RCSectionTagRelation GetSectionTagRelation(string secCode,int tagNumber)
        {
            RCSectionTagRelation result = new RCSectionTagRelation();
            //   result.tagList = _adminDb.GetTagNotinSection(secCode, tagNumber).Result;
            result.tagList = _bookDb.GetTagList().Result;
            result.sectionTagList = _adminDb.GetSectionTag(secCode, tagNumber).Result;
            return result;
        }

        public List<RMsgContent_System> QueryAllSystemNotification()
        {
            return _adminDb.QueryAllSystemNotification();
        }

        public void SaveSystemNotification(EMsgContent_System newContent)
        {
            _adminDb.SaveSystemNotification(newContent);

        }

        public bool SaveSectionTag(ESection section, List<ETag> tagList)
        {
          
            var result = _adminDb.Db.Ado.UseTran(() =>
            {
               
               var r = _adminDb.DeleteRangeByExp(s => s.SectionCode == section.Code);
               r.Wait();
               var r2 =_adminDb.AddRangeTagToSection(section, tagList);
             
               
            });
          
           // var result = trans.Result;
          
            if (result.IsSuccess == false)
                throw new Exception(result.ErrorMessage);
                     
            return result.IsSuccess;
        }
    }
}
