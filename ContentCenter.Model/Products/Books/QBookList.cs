using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class QBookList:QueryPager
    {
        public QBookList_Type QueryType { get; set; }
    }
}
