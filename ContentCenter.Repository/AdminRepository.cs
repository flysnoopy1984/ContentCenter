using ContentCenter.IRepository;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class AdminRepository: BaseRepository<ESectionTag>,IAdminRepository
    {
        public AdminRepository(ISqlSugarClient[] sugarClient)
           : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public int AddRangeTagToSection(ESection section, List<ETag> tagList)
        {
           List<ESectionTag> objList = new List<ESectionTag>();
           foreach(var tag in tagList)
           {
                objList.Add(new ESectionTag
                {
                    SectionCode = section.Code,
                    //SectionName = section.Title,
                    TagCode = tag.Code,
                  //  TagName = tag.Name,
                });
           }
          
             return base.AddRange(objList);
        }


        public Task<List<RSectionTag>> GetSectionTag(string secCode, int number)
        { 
            var q = Db.Queryable<ESectionTag,ETag,EBookTag>((st,t,bt)=> new object[]
            {
                JoinType.Left,st.TagCode == t.Code,
                JoinType.Inner,t.Code == bt.TagCode,
               
            })
            .GroupBy((st, t, bt) => new
            {
                t.Code,
                t.Name,
                st.SectionCode,
             //   st.SectionName
            })
             .Select((st, t, bt) => new RSectionTag
             {
                 SectionCode = st.SectionCode,
              //   SectionName = st.SectionName,
                 TagCode = t.Code,
              //   TagName = t.Name,
                 TagCount = SqlFunc.AggregateCount(t.Code)
             })
             .Where(st=>st.SectionCode == secCode)
             .MergeTable()
             .OrderBy(t => t.TagCount, OrderByType.Desc);
            if (number > 0)
                return q.Take(number).ToListAsync();
            else
                return q.ToListAsync();
        }

        /// <summary>
        /// exclutedSecCode 空，只要有tag再某个Section中，则被排除
        /// </summary>
        /// <param name="exclutedSecCode"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public Task<List<RTag>> GetTagNotinSection(string exclutedSecCode="", int number=0)
        {
            var q = Db.Queryable<ETag, EBookTag>((tag, bt) => new object[] {
                JoinType.Inner,tag.Code == bt.TagCode
                })
              .GroupBy((tag, bt) => new
              {
                  tag.Code,
                  tag.Name
              })
              .Select((tag, bt) => new RTag
              {
                  Code = tag.Code,
                  Name = tag.Name,
                  Count = SqlFunc.AggregateCount(tag.Code)

              })
             
              .Where(tag => SqlFunc.Subqueryable<ESectionTag>()
              .WhereIF(!string.IsNullOrEmpty(exclutedSecCode),s=>s.SectionCode == exclutedSecCode)
              .Where(s => s.TagCode == tag.Code).NotAny())

              .MergeTable()
              .OrderBy(t => t.Count, OrderByType.Desc);
            if (number > 0)
                return q.Take(number).ToListAsync();
            else
                return q.ToListAsync();
        }

        public List<RMsgContent_System> QueryAllSystemNotification()
        {
            List<RMsgContent_System> result = Db.Queryable<EMsgContent_System>().
               OrderBy(a => a.CreateDateTime, OrderByType.Desc).
               Select(a=>new RMsgContent_System
               {
                   CreateDateTime = a.CreateDateTime,
                   htmlContent = a.htmlContent,
                   htmlTitle = a.htmlTitle,
                   Id = a.Id
                   
               }).
               ToList();

            return result;
        }

        public void SaveSystemNotification(EMsgContent_System newContent)
        {
            Db.Saveable(newContent).ExecuteCommand();
        }
    }
}
