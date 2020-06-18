using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public  interface IBookRepository: IBaseRepository<EBookInfo>
    {
        Task<List<RBookSimple>> GetSimpleBookByTag(int pageIndex, int pageSize,string tagCode);

        Task<List<RBookSimple>> GetSimpleBookBySection(int pageIndex, int pageSize, string secCode);

        Task<List<ESection>> GetWebSection(SectionType sectionType);

        /// <summary>
        /// 获取Tag列表
        /// </summary>
        /// <param name="number">需要获取Tag的数量</param>
        /// <param name="orderByType">Tag排序</param>
        /// <returns></returns>
        Task<List<RTag>> GetTagList(int number=0, OrderByType orderByType= OrderByType.Desc);

       





    }
}
