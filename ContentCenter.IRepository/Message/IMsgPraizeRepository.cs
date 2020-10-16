using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IMsgPraizeRepository: IBaseRepository<EMsgInfo_Praize>
    {
        Task<ModelPager<VueMsgInfo_Praize>> msgPraizeList(QUserMsg query);

        /// <summary>
        /// (点赞消息内容)
        /// </summary>
        EMsgContent_Praize GetContentPraize_Sync(long refId, PraizeTarget praizeTarget);

      
        bool ExistMsgPraize_Sync(long refId, PraizeTarget praizeTarget, string sendUserId);
       

        long AddContentPraize_Sync(EMsgContent_Praize content);

       
    }
}
