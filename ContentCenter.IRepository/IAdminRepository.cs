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
        /// 批量添加Tag到Section
        /// </summary>
        int AddRangeTagToSection(ESection section, List<ETag> tagList);

        #region 系统公告
        /// <summary>
        /// 新增或更新
        /// </summary>
        /// <param name="newContent"></param>
        public void SaveSystemNotification(EMsgContent_System newContent);

        public List<RMsgContent_System> QueryAllSystemNotification();
        #endregion

    }
}
