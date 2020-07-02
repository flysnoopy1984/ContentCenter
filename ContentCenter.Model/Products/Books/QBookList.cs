using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class QBookList:QueryPager
    {
        public QBookList_Type QueryType { get; set; }

        public  string Code { get; set; }

        public int HighScoreTop { get; set; } = 500;
    }
}
