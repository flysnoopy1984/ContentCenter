using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccMsgInfoOverview")]
    public class EMsgInfoOverview
    {
        [SugarColumn(IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(IsPrimaryKey = true, Length = 32)]
        public string userId { get; set; }

        /// <summary>
        /// 通知总数
        /// </summary>
        public int notificationTotal { get; set; } = 0;

        public int nPraize { get; set; } = 0;
        public int nComment { get; set; } = 0;

        public int nReply { get; set; } = 0;

        [SugarColumn(IsNullable = true)]
        public int nSystem { get; set; } = 0;

        /// <summary>
        /// 消息总数
        /// </summary>
        public int messageTotal { get; set; } = 0;

        [SugarColumn(DefaultValue = "0")]
        public int readPraize { get; set; } = 0;

        [SugarColumn(DefaultValue = "0")]
        public int readComment { get; set; } = 0;

        [SugarColumn(DefaultValue = "0")]
        public int readReply { get; set; } = 0;

       
    }
}
