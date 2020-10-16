using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class SubmitReply
    {
        public string bookCode { get; set; }
        public long commentId { get; set; }

        public long replyId { get; set; } = -1;

        /// <summary>
        /// 当是回复的回复时使用，指回复（非评论）人的Id
        /// </summary>
        public string replyAuthorId { get; set; }

        /// <summary>
        /// 当是回复的回复时使用，指回复（非评论）人的Name
        /// </summary>
        public string replyAuthorName { get; set; }

        public string content { get; set; }

        public string userId { get; set; }

        /// <summary>
        /// 点赞人 nickname(用于发送消息)
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 点赞人 头像(用于发送消息)
        /// </summary>
        public string userHeaderUrl { get; set; }
    }
}
