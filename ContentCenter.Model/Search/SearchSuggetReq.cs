using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class SearchSuggetReq
    {
        public string userId { get; set; }

        public int MaxLine { get; set; } = 15;

        public string inputWord { get; set; }
    }
}
