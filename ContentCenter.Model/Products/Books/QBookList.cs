using ContentCenter.Model.BaseEnum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class QBookList:QueryPager
    {
        [JsonProperty("queryType")]
        public QBookList_Type QueryType { get; set; }

        [JsonProperty("queryCode")]
        public  string Code { get; set; }

        public int HighScoreTop { get; set; } = 500;
    }
}
