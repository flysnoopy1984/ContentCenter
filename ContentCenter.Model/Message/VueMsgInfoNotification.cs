using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ContentCenter.Model
{
    public class VueMsgInfoNotification
    {
        public string dateTime
        {
            get
            {
                return dbDateTime.ToString("yyyy-MM-dd");
            }
        }
        public string senderHeaderUrl { get; set; }

        public string senderId { get; set; }

        public string senderName { get; set; }

      

        public string bookUrl { get; set; }

        public string bookCode { get; set; }

        public string resCode { get; set; }

        public string resName { get; set; }


        public long commentId { get; set; } = -1;

        public long replyId { get; set; } = -1;

        public string receiveContent { get; set; }

        [JsonIgnore()]
        public DateTime dbDateTime { get; set; }

        /* 点赞消息 使用 Begin */
        public PraizeTarget target { get; set; }

        public string targetName
        {
            get
            {
                switch (target)
                {
                    case PraizeTarget.Comment:
                        return "评论";
                    case PraizeTarget.CommentReply:
                        return "回复";
                    case PraizeTarget.Resource:
                        return "资源";
                }
                return "";
            }
        }
        public string origContent { get; set; }


        /* 点赞消息 使用 End */

        public NotificationStatus NotificationStatus { get; set; }

        //更新消息状态使用
        public long msgId { get; set; }
    }
}
