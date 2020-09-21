using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class QUserTrans: QueryPager
    {
        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
        public string userId { get; set; }
    }
}
