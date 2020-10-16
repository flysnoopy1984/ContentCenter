using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ContentCenter.Model
{
    public class VueMsgInfo_Praize
    {
        public string dateTime
        {
            get
            {
                return dbDateTime.ToString("yyyy-MM-dd");
            }
        }

        public string senderId { get; set; }

        public string senderName { get; set; }

        public PraizeTarget target { get; set; }

        public string targetName { get; set; }

        public string bookUrl { get; set; }

        public string bookCode { get; set; }

        public string content { get; set; }


        [JsonIgnore()]
        public DateTime dbDateTime { get; set; }
    }
}
