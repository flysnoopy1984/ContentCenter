using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccCommentRes")]
    public class EComment_Res : PraizeBaseMasterTable
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public long Id { get; set; }
        public string refCode { get; set; }

        [SugarColumn(Length = 400, ColumnDataType = "nvarchar")]
        public string content { get; set; }

        [SugarColumn(Length = 32)]
        public string authorId { get; set; }

        /// <summary>
        /// 回复总数
        /// </summary>
        public int replyNum { get; set; }
    }
}

