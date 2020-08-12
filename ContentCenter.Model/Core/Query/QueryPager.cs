using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class QueryPager
    {
        public QueryPager() { }
        public QueryPager(int _pageIndex,int _pageSize) {
            pageIndex = _pageIndex;
            pageSize = _pageSize;
        }
        public int pageIndex { get; set; }
        public int pageSize { get; set; } = 15;
    }
}
