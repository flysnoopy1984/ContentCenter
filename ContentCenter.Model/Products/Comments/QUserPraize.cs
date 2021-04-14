using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;
using IQB.Util.Models;

namespace ContentCenter.Model
{
    public class QUserPraize: QueryPager
    {
        public string userId { get; set; }
        public PraizeTarget praizeTarget { get; set; }
    }
}
