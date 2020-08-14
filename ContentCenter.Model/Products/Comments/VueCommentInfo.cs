using ContentCenter.Model.BaseEnum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueCommentInfo
    {
        public long commentId { get; set; }

        public string authorId { get; set; }
        public string authorName { get; set; }

        public string headerUrl { get; set; }

        public string content { get; set; }

       

        private string _dateTime;
        public string dateTime
        {
            get
            {
                return CreateDateTime.ToString("yyyy-MM-dd HH:mm");
            }
            set { _dateTime = value; }
        }

        [JsonProperty(IsReference = false)]
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int goodNum { get; set; }

        public int replyNum { get; set; }

        public PraizeType userPraizeType { get; set; } = PraizeType.noPraize;







    }
}
