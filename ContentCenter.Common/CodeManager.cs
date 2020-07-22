using IQB.Util;
using System;

namespace ContentCenter.Common
{
    public class CodeManager
    {
        /// <summary>
        /// 用户Guid
        /// </summary>
        /// <returns></returns>
        public static string UserCode()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 资源Guid
        /// </summary>
        /// <returns></returns>
        public static string ResCode(string fileType, int resType,string refCode,string owner)
        {
            var o = owner.Length > 6 ? owner.Substring(0, 6) : owner;
            var t = DateTime.Now.ToString("yyyyMMddhhmmss");
            return $"{refCode}-{resType}_{fileType}-{o+t}";
        }
    }
}
