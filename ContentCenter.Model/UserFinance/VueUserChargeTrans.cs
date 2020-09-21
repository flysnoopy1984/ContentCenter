using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueUserChargeTrans
    {
        public decimal amount { get; set; }
       
        public string dateTime
        {
            get
            {
                return dbDateTime.ToString("yyyy-MM-dd");
            }
        }
        public int rate { get; set; }
        public int point { get; set; }

        [JsonIgnore()]
        public DateTime dbDateTime { get; set; }
    }
}
