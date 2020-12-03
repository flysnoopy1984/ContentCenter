using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueMsgInfoOverview
    {
        public string userId { get; set; }

        /// <summary>
        /// 消息总数
        /// </summary>
        public int messageTotal { get; set; } = 0;
        /// <summary>
        /// 通知总数
        /// </summary>
        public int notificationTotal { get; set; } = 0;

        public int nPraize { get; set; } = 0;
        public int nComment { get; set; } = 0;

        public int nReply { get; set; } = 0;

        //系统通知
        public int nSystem { get; set; } = 0;



    }
}
