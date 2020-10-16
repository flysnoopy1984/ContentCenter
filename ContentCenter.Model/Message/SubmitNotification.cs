using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class SubmitNotification
    {
     
        public string sendId { get; set; }
        public string receiverId { get; set; }

        public NotificationType notificationType { get; set; }
        
        public PraizeTarget PraizeTarget { get; set; }

        public long relatedId { get; set; }

        /// <summary>
        /// 暂时不用
        /// </summary>
        public List<string> atIdList { get; set; }
    }
}
