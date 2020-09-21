using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util;
using IQB.Util.Models;
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
        private IUserBookRepository _userBookRepository;
       
        public BookServices(IBookRepository bookRepository, ISectionRepository sectionRepository,
            IUserBookRepository userBookRepository)
          : base(bookRepository)
        {
            _userBookRepository = userBookRepository;
            _sectionRepository = sectionRepository;
            _bookDb = bookRepository;
         
        }

        public ESection GetSection(string secCode)
        {
            return _sectionRepository.GetByKey(secCode).Result;
        }

        public ModelPager<RBookList> GetBookListPager(QBookList query)
        {
            ModelPager<RBookList> result = new ModelPager<RBookList>();
            result.pageIndex = query.pageIndex;
            result.pageSize = query.pageSize;

            RefAsync<int> totalNumber = new RefAsync<int>();
            if (query.QueryType == QBookList_Type.Tag)
            {
                result.datas = _bookDb.GetBookListByTag(query.pageIndex, query.pageSize, query.Code, totalNumber).Result;
                result.totalCount = totalNumber;
                return result;
            }   
            else if(query.QueryType == QBookList_Type.Section)
            {
                ESection section = _sectionRepository.GetByKey(query.Code).Result;
                if(section!=null)
                {
                    if(section.SectionType == SectionType.Column)
                    {
                        result.datas =  _bookDb.GetBookListBySection_ST(query.pageIndex, query.pageSize, query.Code, totalNumber).Result;
                        result.totalCount = totalNumber;
                        return result;
                    }
                    else
                    {
                        if (section.Code == WebSection.NewExpress)
                        {
                            result.datas = _bookDb.GetBookListBySection_DT(query.pageIndex, query.pageSize, query.Code, totalNumber).Result;
                            result.totalCount = totalNumber;
                            return result;
                        }
                        else if (section.Code == WebSection.ResDownLoad)
                        {
                            result.datas = _bookDb.GetBookListBySection_Resource(query.pageIndex, query.pageSize,totalNumber).Result;
                            result.totalCount = totalNumber;
                            return result;
                        }
                        else if (section.Code == WebSection.HighScore)
                        {
                            result.datas = _bookDb.GetBookListBySection_HighScroe(query.pageIndex, query.pageSize, totalNumber,defaultTop:query.HighScoreTop).Result;
                            if (totalNumber > query.HighScoreTop)
                            {
                                totalNumber = new RefAsync<int>(query.HighScoreTop);
                            }
                               
                            result.totalCount = totalNumber;
                            return result;
                        }  
                    }
                }
            }
          
            return null;
           // return _bookDb.GetSimpleBookPager(pageIndex, pageSize, bookSection).Result;
        }

        public List<RTag> GetTagList(int number, OrderByType orderByType)
        {
            return _bookDb.GetTagList(number, orderByType).Result;
        }

        public Dictionary<string, List<ESection>> GetWebSection(SectionType sectionType)
        {
            var list =  _bookDb.GetWebSection(sectionType).Result;
            var result = new Dictionary<string, List<ESection>>();
            result["Book"] = new List<ESection>();
            result["Column"] = new List<ESection>();
            if(list.Count>0)
            {
                if (sectionType == SectionType.All)
                {
                    foreach (var st in list)
                    {
                        var secName = st.SectionType.ToString();
                        result[secName].Add(st);
                    }
                }
                else
                {
                    var secName = list[0].SectionType.ToString();
                    result[secName] = list;
                }
                   
            }
           
            return result;
        }

        public RBookInfo Info(string bookCode,string userId)
        {
            RBookInfo bi = new RBookInfo();
            bi.bookInfo = _bookDb.GetByKey(bookCode).Result;
            bi.IsUserFav = _userBookRepository.HasFavBook(bookCode, userId).Result>0?true:false;
            return bi;
        }

      
    }
}
