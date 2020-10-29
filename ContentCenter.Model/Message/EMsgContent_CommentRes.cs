using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccMsgContent_CommentRes")]
    public class EMsgContent_CommentRes: BaseMsgContent
    {
        /// <summary>
        /// 资源Code,
        /// </summary>
        [SugarColumn(Length = 50,IsPrimaryKey =true)]
        public new string ResCode { get; set; }

       
    }
}
