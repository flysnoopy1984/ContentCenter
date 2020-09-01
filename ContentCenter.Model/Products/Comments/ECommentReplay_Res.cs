using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccCommentReply_Res")]
    public class ECommentReply_Res: PraizeBaseMasterTable
    {
        [SugarColumn(IsIdentity = true,IsPrimaryKey =true)]
        public long Id { get; set; }

        public long commentId { get; set; }

        public ReplyType replyType { get; set; }

        [SugarColumn(Length = 400, ColumnDataType = "nvarchar")]
        public string content { get; set; }

        [SugarColumn(Length = 32)]
        public string authorId { get; set; }

        [SugarColumn(IsNullable = true, DefaultValue = "-1")]
        public long replyId { get; set; } = -1;

        [SugarColumn(Length = 32, IsNullable = true)]
        public string replyAuthorId { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string replyName { get; set; }
    }
}
