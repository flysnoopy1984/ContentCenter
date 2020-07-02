﻿using ContentCenter.Model;
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
        Task<List<RBookList>> GetBookListByTag(int pageIndex, int pageSize,string tagCode, RefAsync<int> totalNumber);

        /// <summary>
        /// 通过SectionTag 获取Tab.再通过BookTag获取对应的Book
        /// </summary>
       
        Task<List<RBookList>> GetBookListBySection_ST(int pageIndex, int pageSize, string secCode, RefAsync<int> totalNumber);

        /// <summary>
        /// 通过DataSection表直接获取Section和Book关系
        /// </summary>
        Task<List<RBookList>> GetBookListBySection_DT(int pageIndex, int pageSize, string secCode, RefAsync<int> totalNumber);

        /// <summary>
        /// 高分榜(默认前500)
        /// </summary>
        Task<List<RBookList>> GetBookListBySection_HighScroe(int pageIndex, int pageSize, RefAsync<int> totalNumber,int defaultTop);

        Task<List<ESection>> GetWebSection(SectionType sectionType);

        /// <summary>
        /// 获取Tag列表
        /// </summary>
        /// <param name="number">需要获取Tag的数量</param>
        /// <param name="orderByType">Tag排序</param>
        /// <returns></returns>
        Task<List<RTag>> GetTagList(int number=0, OrderByType orderByType= OrderByType.Desc);



        Task<string> TestSql();



    }
}