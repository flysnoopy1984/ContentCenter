using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueUserPraize
    {
        //public int praizeType { get; set; }
        //public string praizeTypeText { get; set; }

        /// <summary>
        /// 资源就是 Code 评论和回复就是Id
        /// </summary>
        public string code { get; set; }
        public string bookCode { get; set; }
        public string bookName { get; set; }
        public string bookUrl { get; set; }

        public string refContent { get; set; }

        public string content { get; set; }

     //   public string resName { get; set; }
        public string dateTime
        {
            get
            {
                return CreateDateTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        [JsonProperty(IsReference = false)]
        public DateTime CreateDateTime { get; set; }

        public bool IsEdit { get; set; } = false;
    }
}
