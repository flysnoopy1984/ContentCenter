using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    /// <summary>
    /// 用户积分
    /// </summary>
    [SugarTable("ccUserPointsTrans")]
    public class EUserPointsTrans
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 32)]
        public string userId { get; set; }

        [SugarColumn(DefaultValue = "0")]
        public int point { get; set; } = 0;

        public PointChangeType changeType { get; set; }

        public DateTime createdDateTime { get; set; } = DateTime.Now;
    }
}
