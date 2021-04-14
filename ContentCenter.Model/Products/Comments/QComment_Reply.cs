using System;
using System.Collections.Generic;
using System.Text;
using IQB.Util.Models;

namespace ContentCenter.Model
{
    public class QComment_Reply: QueryPager
    {
        public long commentId { get; set; }

        public long fixedReplyId { get; set; }

        public string reqUserId { get; set; }
    }
}
