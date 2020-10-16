using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccMsgInfo_ReplyRes")]
    public class EMsgInfo_ReplyRes: BaseNotification
    {
       
        /// <summary>
        /// 回复的评论。
        /// </summary>
        public long CommentId { get; set; }

        /// <summary>
        /// 只有当回复回复时有效
        /// </summary>
        public long ReplyReplyId { get; set; }

        /// <summary>
        /// 当前发送的ReplyId
        /// </summary>
        public long ReplyId { get; set; }

        [SugarColumn(Length = 200)]
        public string ReceiveContent { get; set; }
    }
}
