using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccMsgInfo_System")]
    public class EMsgInfo_System
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 32)]
        public string ReceiveUserId { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 通知状态，暂时直接发送，不做后台过滤
        /// </summary>
        public NotificationStatus NotificationStatus { get; set; } = NotificationStatus.sent;

        /// <summary>
        /// 是否已经推送给App端(暂无)
        /// </summary>
        public bool IsPushToApp { get; set; } = false;


        public long ContentId { get; set; }
    }
}
