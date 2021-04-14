using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.Baidu
{
    public class panQueryFile:QueryPager
    {

        public string accessToken { get; set; }

        public string rootPath { get; set; } = "/";

        public List<string> fsIds { get; set; }

        //Baidu 是否转换成了 ccBook,或者仅获取了DouBan Code.
        public bool showConvertInfo { get; set; } = false;

        public bool queryOnlyFromDB { get; set; } = false;

        
        public int querySearchResultNum { get; set; } = 0;

        public panConvertStatus convertStatus { get; set; } = panConvertStatus.All;

       

       
    }
}
