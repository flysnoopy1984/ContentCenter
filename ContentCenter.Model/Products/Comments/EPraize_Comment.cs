using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccPraize_Comment")]
    public class EPraize_Comment:EPraizeDetail
    {
       // public EPraize_Comment():base(PraizeTarget.Comment) { }
        public long commentId { get; set; }

        /// <summary>
        /// ResCode
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string RefCode { get; set; }
    }
}
