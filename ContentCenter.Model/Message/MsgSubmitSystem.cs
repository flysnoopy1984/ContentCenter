using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class MsgSubmitSystem
    {
        public long contentId { get; set; }

        public string htmlContent { get; set; }
        public string htmlTitle { get; set; }

        /// <summary>
        /// 发送个人还是组
        /// </summary>
        public SystemNoteTarget systemNoteTarget { get; set; }

        public Group_Notification receiveGroupId { get; set; }

        public string receiveUserId { get; set; }
    }
}
