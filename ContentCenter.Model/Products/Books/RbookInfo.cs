using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class RBookInfo
    {
        public EBookInfo bookInfo { get; set; }
        public bool IsUserFav { get; set; }

        public RBookNextPrev bookNextPrev { get; set; }
    }
}
