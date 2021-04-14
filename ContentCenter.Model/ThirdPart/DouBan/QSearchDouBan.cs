using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.DouBan
{
    public class QSearchDouBan:QueryPager
    {
        //查询多少个搜索结果
        public int queryNum { get; set; }
    }
}
