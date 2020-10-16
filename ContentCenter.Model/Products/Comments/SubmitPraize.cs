using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class SubmitPraize
    {
        public PraizeTarget praizeTarget { get; set; }

        /// <summary>
        /// 修改后，其他点赞也要传入bookCode
        /// </summary>
        public string bookCode { get; set; }

        /// <summary>
        /// refCode如果是资源，则parent是BookCode
        /// </summary>
        public string parentRefCode { get; set; }
        
        /// <summary>
        /// 对于资源 refCode就是资源的主键Code
        /// </summary>
        public string refCode { get; set; }

        /// <summary>
        /// 点赞人
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 点赞人 nickname(用于发送消息)
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 点赞人 头像(用于发送消息)
        /// </summary>
        public string userHeaderUrl { get; set; }

        /// <summary>
        /// 仅当资源点赞有效
        /// </summary>
        public long resId { get; set; }

        public OperationDirection praizeDirection { get; set; } 
        public PraizeType praizeType { get; set; }
    }
}
