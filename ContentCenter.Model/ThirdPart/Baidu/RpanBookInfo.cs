using ContentCenter.Model.ThirdPart.DouBan;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.Baidu
{
    public class RpanBookInfo
    {
        public panBookInfo panBookInfo { get; set; }

        public List<RSearchOneBookResult> searchList { get; set; }
    }
}
