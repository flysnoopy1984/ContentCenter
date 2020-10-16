using ContentCenter.Model.BaseEnum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class SubmitComment
    {
        public CommentTarget commentTarget { get; set; } = CommentTarget.Resource;

        /// <summary>
        /// 资源Code
        /// </summary>
        public string refCode { get; set; }

        public string content { get; set; }

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
        /// BookCode 
        /// </summary>
        public string parentRefCode { get; set; }

        /// <summary>
        /// 对于资源的点赞
        /// </summary>
        //[JsonProperty("praizeType")]
        //public PraizeType PraizeType { get; set; }

    }
}
