using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IAdminRepository:IBaseRepository<ESectionTag>
    {
        /// <summary>
        /// 获取Section下的Tags
        /// </summary>
        /// <param name="secCode"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<List<RSectionTag>> GetSectionTag(string secCode, int number);

        /// <summary>
        /// 获取所有不在某个Section下的Tag
        /// </summary>
        /// <returns></returns>
        Task<List<RTag>> GetTagNotinSection(string exclutedSecCode,int number);

     

        /// <summary>
        /// 删除所有Section中过的Tag
        /// </summary>
        /// <param name="secCode"></param>
        /// <param name="tagList"></param>
        /// <returns></returns>
      //  Task<bool> DeleteAllTagInSection(string secCode, List<ETag> tagList);

        /// <summary>
        /// 批量添加Tag到Section
        /// </summary>
        Task<int> AddRangeTagToSection(ESection section, List<ETag> tagList);
        
    }
}
