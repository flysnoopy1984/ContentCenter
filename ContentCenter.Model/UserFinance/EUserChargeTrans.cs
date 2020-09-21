using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    /// <summary>
    /// 充值记录
    /// </summary>
    [SugarTable("ccUserChargeTrans")]
    public class EUserChargeTrans
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 32)]
        public string userId { get; set; }

        [SugarColumn(DecimalDigits = 2, DefaultValue = "0.00")]
        public decimal money { get; set; } = 0;

        [SugarColumn(IsNullable =true)]
        /// <summary>
        /// 汇率 人名币对比积分
        /// </summary>
        public int rate { get; set; } = 10;

        /// <summary>
        /// 充值可得积分
        /// </summary>
        [SugarColumn(IsNullable = true,DefaultValue ="0")]
        public int point { get; set; } = 0;

        public DateTime createdDateTime { get; set; } = DateTime.Now;
    }
}
