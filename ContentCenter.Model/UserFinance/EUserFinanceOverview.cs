using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccUserFinanceOverview")]
    public class EUserFinanceOverview
    {
        [SugarColumn(IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 32, IsPrimaryKey = true)]
        public string userId { get; set; }

        [SugarColumn(DecimalDigits = 2, DefaultValue = "0.00")]
        public decimal money { get; set; } = 0;

        [SugarColumn(DecimalDigits = 2, DefaultValue = "0.00")]
        public decimal commission { get; set; } = 0;

        [SugarColumn(DefaultValue = "0")]
        public int chargePoint { get; set; } = 0;

       

        public DateTime pointEffectDate { get; set; }

        [SugarColumn(DefaultValue = "0")]
        public int fixedPoint { get; set; } = 0;


    }
}
