using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class QComment_Reply: QueryPager
    {
        public long commentId { get; set; }
    }
}
