using ContentCenter.Model.BaseEnum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.Products.Res
{
    public class VueBookRes
    {
        public string bookCode { get; set; }
        public ResType resType { get; set; }

        public string fileType { get; set; }

        public string origFileName { get; set; }

        public string url { get; set; }

        [JsonProperty(IsReference = false)]
        public DateTime CreateDateTime { get; set; }

        public string uploadDateTime
        {
            get
            {
                return CreateDateTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
    }
}
