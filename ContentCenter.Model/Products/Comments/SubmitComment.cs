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
        public string refCode { get; set; }

        public string content { get; set; }

        public string userId { get; set; }

        public string parentRefCode { get; set; }

        /// <summary>
        /// 对于资源的点赞
        /// </summary>
        //[JsonProperty("praizeType")]
        //public PraizeType PraizeType { get; set; }

    }
}
