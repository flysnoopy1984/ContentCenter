using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IResourceReponsitory: IBaseRepository<EResourceInfo>
    {

        bool LogicDelete(string resCode);

        Task<int> SameResCount(string userId,string refCode, ResType resType, string fileType, bool includeDelete = false);

        Task<ModelPager<VueResInfo>> GetResByRefCode(QRes qRes);


        /// <summary>
        /// 先分组获取用户资源对应的书本
        /// 获取书本资源列表，再次获取书本对应的资源。整理数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ModelPager<VueUserRes>> queryUserRes_GroupByBook(QUserRes query);
       

        int logRequireRes(string resCode, string requireUserId);

        bool addRequireResNum(string resCode);

        /// <summary>
        /// 根据资源获取拥有着信息(发消息使用)
        /// ID headerUrl Name
        /// </summary>
        /// <param name="resCode"></param>
        /// <returns></returns>
        EUserInfo getResoureOwnerId(string resCode);

       


    }
}
