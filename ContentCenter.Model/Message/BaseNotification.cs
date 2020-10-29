using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public abstract class BaseNotification
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// xxx 对评论点了赞，xxx回复了评论，xxx对资源做了评论。
        /// sendUserId 就是xxx
        /// </summary>
        [SugarColumn(Length = 32)]
        public string SendUserId { get; set; }
        public string SendName { get; set; }

        public string SendHeaderUrl { get; set; }

        [SugarColumn(Length = 32)]
        public string ReceiveUserId { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 通知状态，暂时直接发送，不做后台过滤
        /// </summary>
        public NotificationStatus NotificationStatus { get; set; }= NotificationStatus.sent;

        /// <summary>
        /// 是否已经推送给App端(暂无)
        /// </summary>
        public bool IsPushToApp { get; set; } = false;

        /// <summary>
        /// 消息内容Id
        /// </summary>
      //  public long ContentId { get; set; }
      
    }
}
