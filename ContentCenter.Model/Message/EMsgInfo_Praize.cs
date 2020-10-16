using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccMsgInfo_Praize")]
    public class EMsgInfo_Praize:BaseNotification
    {
        
        public PraizeTarget PraizeTarget { get; set; }

        /// <summary>
        /// 针对哪个点赞，资源用Id(而不是主键Code),评论Id，回复Id
        /// </summary>
        public long RefId { get; set; }

        public long PraizeId { get; set; }
    }
}
