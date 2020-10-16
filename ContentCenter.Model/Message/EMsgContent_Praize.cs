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
        /// 针对哪个点赞，资源用Id(而不是主键Code),评论Id，回复Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long RefId { get; set; }

       
     
       

    }
}
