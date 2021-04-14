using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccSysConfig")]
    public class ESysConfig
    {
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(Length = 32, IsPrimaryKey = true)]
        public string sysCode { get; set; }

        /// <summary>
        /// 充值-积分有效期
        /// </summary>
        public int pointEffectDay_Charge { get; set; } = 365;

        /// <summary>
        /// 注册-积分有效期
        /// </summary>
        public int pointEffectDay_Register { get; set; } = 3 * 30;

        public int pointGive_Regiser { get; set; } = 20;

        /// <summary>
        /// 积分和钱的兑换率
        /// </summary>
        public int pointMoneyRate { get; set; } = 10;

        public int baiduPanTokenExpiredDay { get; set; } = 28;

        
    }
}
