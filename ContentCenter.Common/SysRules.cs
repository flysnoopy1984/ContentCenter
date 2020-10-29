using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Common
{
    public static class SysRules
    {
        //50个点赞后评论会置顶
        public const int OrderByPraizeStartNum = 5;
        public static string ccDateTimeStr(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd hh:mm");
        }
    }
}
