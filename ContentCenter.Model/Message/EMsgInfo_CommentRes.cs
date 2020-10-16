using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccMsgInfo_CommentRes")]
    public class EMsgInfo_CommentRes:BaseNotification
    {
        [SugarColumn(Length = 50, ColumnDataType = "varchar")]
        public string resCode { get; set; }
        public long CommentId { get; set; }

        /// <summary>
        /// 发起者给出的消息（资源评论内容）
        /// </summary>
        [SugarColumn(Length = 200)]
        public string ReceiveContent { get; set; }
    }
}
