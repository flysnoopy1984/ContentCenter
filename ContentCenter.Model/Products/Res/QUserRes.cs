using System;
using System.Collections.Generic;
using System.Text;
using IQB.Util.Models;

namespace ContentCenter.Model
{
    public class QUserRes: QueryPager
    {
        public string userId { get; set; }

        bool includeDelete { get; set; } = false;
    }
}
