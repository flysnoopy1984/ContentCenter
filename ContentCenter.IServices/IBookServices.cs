using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IServices
{
    public interface IBookServices:IBaseServices<EBookInfo>
    {
        /// <summary>
        /// 获取单本书的详细信息
        /// </summary>
        /// <param name="bookCode"></param>
        /// <returns></returns>
        RBookInfo Info(string bookCode, string userId);

        /// <summary>
        /// 根据Section TagCode 获取书列表
        /// </summary>
        /// <returns></returns>
        ModelPager<RBookList> GetBookListPager(QBookList query);

        /// <summary>
        /// /获取网站的Section
        /// </summary>
        /// <param name="sectionType"></param>
        /// <returns></returns>
        Dictionary<string, List<ESection>> GetWebSection(SectionType sectionType);

        /// <summary>
        /// 获取Tag列表
        /// </summary>
        /// <param name="number">需要获取Tag的数量</param>
        /// <param name="orderByType">Tag排序</param>
        /// <returns></returns>
       List<RTag> GetTagList(int number, OrderByType orderByType);

        /// <summary>
        /// 获取单个Section
        /// </summary>
        /// <param name="secCode"></param>
        /// <returns></returns>
        ESection GetSection(string secCode);

       

    }
}
