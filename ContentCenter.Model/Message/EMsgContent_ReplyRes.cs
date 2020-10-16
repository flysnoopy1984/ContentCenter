using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccMsgContent_ReplyRes")]
    public class EMsgContent_ReplyRes: BaseMsgContent
    {
        /// <summary>
        /// OrigContent 存Comment信息
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long CommentId { get; set; }


        /// <summary>
        /// 如果是回复的回复有值，如果是回复则-1；
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long ReplyId { get; set; } = -1;

        /// <summary>
        /// 如果是恢复的回复，存入回复信息，否则为空
        /// </summary>
        [SugarColumn(Length = 200,IsNullable =true)]
        public string OrigReplyContent { get; set; }


    }
}
