using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class SearchReq : QueryPager
    {
        public string userId { get; set; }
        public string keyword { get; set; }
    }
}
