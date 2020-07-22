using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContentCenter.Common
{
    public  class OssKeyManager
    {
        public static string BookKey(string localfilePath,string refCode,string userId)
        {
            FileInfo fi = new FileInfo(localfilePath);
            var fn = fi.Name;
            if (userId.Length > 10) 
                userId =userId.Substring(0, 10);
            if (fn.Length > 80)
            {
             
                var noExtFn = Path.GetFileNameWithoutExtension(localfilePath);
                int maxLen = 80 - fi.Extension.Length;
                noExtFn = noExtFn.Substring(0, maxLen);
                fn = noExtFn + fi.Extension;
            }
            return "Books/"+ refCode+"/" + fi.Extension.Remove(0, 1) + "/" + userId + "_" + fn;
        }

        public static string BookDeletedKey(string origKey)
        {
            return "Deleted/" + origKey;
           
        }
    }
}
