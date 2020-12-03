using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ContentCenter.Model
{
    public class VueSystemNotification
    {
        public string htmlContent { get; set; }

        public string htmlTitle { get; set; }

        public string dateTime
        {
            get
            {
                return dbDateTime.ToString("yyyy-MM-dd");
            }
        }

        [JsonIgnore()]
        public DateTime dbDateTime { get; set; }

        public NotificationStatus NotificationStatus { get; set; }

        //更新消息状态使用

        public long msgId { get; set; }
        public long contentId { get; set; }


    }
}
