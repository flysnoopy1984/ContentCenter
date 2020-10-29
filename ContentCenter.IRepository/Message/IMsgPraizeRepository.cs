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

        /// <summary>
        /// (点赞消息内容)
        /// </summary>
        EMsgContent_Praize GetContentPraize_Sync(string refId, PraizeTarget praizeTarget);

      
        bool ExistMsgPraize_Sync(string refId, PraizeTarget praizeTarget, string sendUserId);
       

        long AddContentPraize_Sync(EMsgContent_Praize content);

        /// <summary>
        /// 查询用户点赞消息
        /// </summary>
        Task<ModelPager<VueMsgInfoNotification>> queryUserPraize(QMsgUser query);

        //更新信息状态为已读
        int UpdateMsgStatus(SubmitUnReadMsgIdList submitData);



    }
}
