using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IServices
{
    public interface IMessageServices: IBaseServices<EMsgInfo_Praize>
    {
        void SendNotification(SubmitNotification submitNotification);

        /// <summary>
        /// 资源，评论，回复 点赞后的通知
        /// </summary>
        /// <param name="msgSubmitPraize"></param>
        void CreateNotification_Praize(MsgSubmitPraize msgSubmitPraize);

        /// <summary>
        /// 对资源评论后的消息
        /// </summary>
        /// <param name="msgSubmitComment"></param>
        void CreateNotification_Comment(MsgSubmitComment msgSubmitComment);

        void CreateNotification_Reply(MsgSubmitReply msgSubmitReply);

        VueMsgInfoOverview GetUserMsgOverview(string userId);

        ModelPager<VueMsgInfoNotification> QueryUserNotifictaion(QMsgUser query);

        //跟新消息到已读
       // ResultNormal updateMsgToRead(SubmitUnReadMsgIdList submitData);

        //查询时后异步更新消息到已读
        void Async_MsgToReadAfterQuery(QMsgUser query, List<VueMsgInfoNotification> queryResult);



    }
}
