using ContentCenter.Model.BaseEnum;

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccMsgContent_Praize")]
    public class EMsgContent_Praize: BaseMsgContent
    {
      
        [SugarColumn(IsPrimaryKey = true)]
        public PraizeTarget PraizeTarget { get; set; }

        /// <summary>
        /// 针对哪个点赞，资源用Code,评论Id，回复Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,Length =50)]
        public string RefId { get; set; }


        [SugarColumn(Length = 200, IsNullable = true)]
        public string OrigContent { get; set; }

        [SugarColumn(IsNullable = true, DefaultValue = "-1")]
        public long CommentId { get; set; }

        [SugarColumn(IsNullable = true, DefaultValue = "-1")]
        public long ReplyId { get; set; }


    }
}
