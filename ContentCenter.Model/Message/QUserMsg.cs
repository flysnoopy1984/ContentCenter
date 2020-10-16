using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class QUserMsg: QueryPager
    {
        /// <summary>
        /// 谁的消息
        /// </summary>
        public string userId { get; set; }

        public NotificationType notificationType { get; set; }
    }
}
