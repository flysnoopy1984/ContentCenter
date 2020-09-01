using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class QUserPraize: QueryPager
    {
        public string userId { get; set; }
        public PraizeTarget praizeTarget { get; set; }
    }
}
