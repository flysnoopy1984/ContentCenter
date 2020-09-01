using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueUserBook
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public string CoverUrl { get; set; }

        public int ResourceCount { get; set; }

        public double Score { get; set; }

        [JsonProperty(IsReference = false)]
        public DateTime CreateDateTime { get; set; }

     
        public string FavDateTime {
            get
            {
                 return CreateDateTime.ToString("yyyy-MM-dd HH:mm");
            }
        
        }

    }
}
