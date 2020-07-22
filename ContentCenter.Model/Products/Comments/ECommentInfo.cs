using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccCommentInfo")]
    public class ECommentInfo: BaseMasterTable
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 200, ColumnDataType = "nvarchar")]
        public string content { get; set; }

        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string Author { get; set; }


        /// <summary>
        /// 回复Id
        /// </summary>
        public long ReplayId { get; set; } 

        /// <summary>
        /// 父节点
        /// </summary>
        public long parentId { get; set; } = -1;
    }
}
