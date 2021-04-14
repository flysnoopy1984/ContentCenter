using ContentCenter.Model.BaseEnum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using IQB.Util.Models;

namespace ContentCenter.Model
{
    public class QBookList:QueryPager
    {
        [JsonProperty("queryType")]
        public QBookList_Type QueryType { get; set; }

        /// <summary>
        /// 对于Search来说是 Keyword,对于Query 来说是书本Code或栏目Code
        /// </summary>
        [JsonProperty("queryCode")]
        public  string Code { get; set; }

        public int HighScoreTop { get; set; } = 500;
    }
}
