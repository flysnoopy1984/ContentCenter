using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ContentCenter.Model
{
    public class VueUserCommissionTrans
    {
     

        public string dateTime
        {
            get
            {
                return dbDateTime.ToString("yyyy-MM-dd");
            }
        }
        public decimal commission { get; set; }
        public CommissionChangeType changeType { get; set; }

        public string changeTypeStr
        {
            get
            {
                switch (changeType)
                {
                    case CommissionChangeType.ad:
                        return "广告佣金";
                    case CommissionChangeType.settle:
                        return "佣金结算";
                }
                return "";
            }
        }
        [JsonIgnore()]
        public DateTime dbDateTime { get; set; }
    }
}
