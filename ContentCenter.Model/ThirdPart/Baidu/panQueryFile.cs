using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.Baidu
{
    public class panQueryFile
    {
        public string accessToken { get; set; }

        public string rootPath { get; set; } = "/";

        public List<string> fsIds { get; set; }
    }
}
