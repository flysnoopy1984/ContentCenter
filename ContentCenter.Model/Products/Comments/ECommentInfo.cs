using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccCommentInfo")]
    public class ECommentInfo: PraizeBaseMasterTable
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public long Id { get; set; }

        public CommentTarget commentTarget { get; set; }

        [SugarColumn(Length = 50)]
        public string refCode { get; set; }

        [SugarColumn(Length = 400, ColumnDataType = "nvarchar")]
        public string content { get; set; }

        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string authorAccount { get; set; }

        /// <summary>
        /// 回复Id
        /// </summary>
        public long replayId { get; set; } 
        

        /// <summary>
        /// 父节点
        /// </summary>
        public long parentId { get; set; } = -1;
    }
}
