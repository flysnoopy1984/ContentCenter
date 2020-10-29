using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class QMsgUser: QueryPager
    {
        /// <summary>
        /// 谁的消息 暂时不支持，从Token中使用
        /// </summary>
        public string userId { get; set; }

        public NotificationType notificationType { get; set; }

        public bool updateMsgToRead { get; set; } = false;
    }
}
