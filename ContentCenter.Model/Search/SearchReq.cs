using System;
using System.Collections.Generic;
using System.Text;
using IQB.Util.Models;

namespace ContentCenter.Model
{
    public class SearchReq : QueryPager
    {
        public string userId { get; set; }
        public string keyword { get; set; }
    }
}
