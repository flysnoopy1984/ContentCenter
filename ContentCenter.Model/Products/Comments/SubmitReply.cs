using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class SubmitReply
    {
        public long commentId { get; set; }

        public long replyId { get; set; }

        public string replyAuthorId { get; set; }
        public string replyAuthorName { get; set; }

        public string content { get; set; }

        public string userId { get; set; }
    }
}
