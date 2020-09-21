using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ContentCenter.Model
{
    public class VueUserPointTrans
    {
        public int point { get; set; }

        public PointChangeType changeType { get; set; }

        public string changeTypeStr
        {
            get
            {
                switch (changeType)
                {
                    case PointChangeType.charge:
                        return "充值获得";
                    case PointChangeType.consume_Download:
                        return "下载消耗";
                    case PointChangeType.newRegister:
                        return "注册赠送";
                      
                }
                return "";
            }
        }

        public string dateTime
        {
            get
            {
                return dbDateTime.ToString("yyyy-MM-dd");
            }
        }

        [JsonIgnore()]
        public DateTime dbDateTime { get; set; }


    }
}
