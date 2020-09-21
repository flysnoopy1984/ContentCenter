using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    /// <summary>
    /// 用户余额
    /// </summary>
    [SugarTable("ccUserBalanceTrans")]
    public class EUserBalanceTrans
    {
        [SugarColumn(IsIdentity = true,IsPrimaryKey =true)]
        public long Id { get; set; }

        [SugarColumn(Length = 32)]
        public string userId { get; set; }


        [SugarColumn(DecimalDigits = 2, DefaultValue = "0.00")]
        public decimal money { get; set; } = 0;

        public BalanceChangeType changeType { get; set; }

        public DateTime createdDateTime { get; set; } = DateTime.Now;


    }
}
