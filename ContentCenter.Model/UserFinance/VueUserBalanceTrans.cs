using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ContentCenter.Model
{
    public class VueUserBalanceTrans
    {
        public decimal money { get; set; }

        public BalanceChangeType changeType { get; set; }

        public string changeTypeStr
        {
            get
            {
                switch (changeType)
                {
                    case BalanceChangeType.charge:
                        return "余额充值";
                    case BalanceChangeType.drawMoney:
                        return "提款";
               
                }
                return "";
            }
        }

        public string dateTime
        {
            get{
                return dbDateTime.ToString("yyyy-MM-dd");
            }
        }

        [JsonIgnore()]
        public DateTime dbDateTime { get; set; }

    }
}
