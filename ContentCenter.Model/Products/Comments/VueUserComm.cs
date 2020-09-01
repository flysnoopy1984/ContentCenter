using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    /// <summary>
    /// 用户用户中心评论展示
    /// </summary>
    public class VueUserComm
    {
        public string bookCode { get; set; }
        public string bookName { get; set; }

        public string resName { get; set; }

        public string content { get; set; }

        public long commentId { get; set; }

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
