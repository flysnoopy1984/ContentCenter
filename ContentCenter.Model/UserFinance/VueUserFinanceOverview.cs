using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueUserFinanceOverview
    {
        public decimal money { get; set; } 
        public int point { get; set; } 

        public int fixPoint { get; set; }
        public decimal commission { get; set; }

        public string effectDate {
            get
            {
                return dbDateTime.ToString("yyyy-MM-dd");
            }
        }

        [JsonIgnore()]
        public DateTime dbDateTime { get; set; }
    }
}
