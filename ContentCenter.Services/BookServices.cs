using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Services
{
    public class BookServices : BaseServices<EBookInfo>, IBookServices
    {
        private IBookRepository _bookDb;
        private ISectionRepository _sectionRepository;
       
        public BookServices(IBookRepository bookRepository, ISectionRepository sectionRepository)
          : base(bookRepository)
        {
            _sectionRepository = sectionRepository;
            _bookDb = bookRepository;
         
        }

        public List<EBookInfo> GetBookPager(int pageIndex, int pageSize)
        {
            try
            {
              //  _bookDb.QueryPager()
            }
            catch(Exception ex)
            {
               // NLogUtil.cc_ErrorTxt("") 
            }
            return new List<EBookInfo>();
        }

        public ESection GetSection(string secCode)
        {
            return _sectionRepository.GetByKey(secCode).Result;
        }

        public List<RBookSimple> GetSimpleBookPager(QBookList query)
        {
            return null;
           // return _bookDb.GetSimpleBookPager(pageIndex, pageSize, bookSection).Result;
        }

        public List<RTag> GetTagList(int number, OrderByType orderByType)
        {
            return _bookDb.GetTagList(number, orderByType).Result;
        }

        public Dictionary<SectionType, List<ESection>> GetWebSection(SectionType sectionType)
        {
            var list =  _bookDb.GetWebSection(sectionType).Result;
            var result = new Dictionary<SectionType, List<ESection>>();
            result[SectionType.Book] = new List<ESection>();
            result[SectionType.Column] = new List<ESection>();
            if(list.Count>0)
            {
                if (sectionType == SectionType.All)
                {
                    foreach (var st in list)
                    {
                        result[st.SectionType].Add(st);
                    }
                }
                else
                    result[list[0].SectionType] = list;
            }
           
            return result;
        }

        public EBookInfo Info(string bookCode)
        {
            return _bookDb.GetByKey(bookCode).Result;
        }

      
    }
}
