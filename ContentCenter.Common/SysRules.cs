using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Common
{
    public static class SysRules
    {
        public static string ccDateTimeStr(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd hh:mm");
        }
    }
}
