using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class SubmitUnReadMsgIdList
    {
        public string userId { get; set; }

        public NotificationType notificationType { get; set; }

        public NotificationStatus targetStatus { get; set; } = NotificationStatus.read;

        private List<long> _msgIdList;
        public List<long> msgIdList {
            get
            {
                if (_msgIdList == null) _msgIdList = new List<long>();
                return _msgIdList;
            }
        }
    }
}
