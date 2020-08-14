using System;
using System.Collections.Generic;
using System.Text;
using ContentCenter.Model.BaseEnum;
using SqlSugar;

namespace ContentCenter.Model
{
    [SugarTable("ccPraize_CommentReply")]
    public class EPraize_CommentReply:EPraizeDetail
    {
       // public EPraize_CommentReply():base(PraizeTarget.CommentReply) { }

        public long replyId { get; set; }
        public long commentId { get; set; }
    }
}
