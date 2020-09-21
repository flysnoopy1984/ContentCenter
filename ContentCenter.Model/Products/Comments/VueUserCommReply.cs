using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueUserCommReply
    {
        public string bookCode { get; set; }
        public string bookName { get; set; }
        public string bookCoverUrl { get; set; }
        public long commentId { get; set; }
        public string commentAuthorId { get; set; }
        public string commentAuthor { get; set; }


        [JsonIgnore()]
        public DateTime pCommentDateTime { get; set; }
        public string commentDateTime{
            get { return pCommentDateTime.ToString("yyyy-MM-dd HH:mm"); }
        }
        public string commentContent { get; set; }
        public long replyId { get; set; }

        public string replyTarget { get; set; }
        public string replyContent { get; set; }

        public string replyDateTime{
            get{return pReplyDateTime.ToString("yyyy-MM-dd HH:mm");}
        }

        [JsonIgnore()]
        public DateTime pReplyDateTime { get; set; }

        public bool IsEdit { get; set; } = false;
    }
}
