using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueUserBook
    {
        public  long id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public string CoverUrl { get; set; }

        public int ResourceCount { get; set; }

        public double Score { get; set; }

        [JsonIgnore()]
        public DateTime CreateDateTime { get; set; }

     
        public string FavDateTime {
            get
            {
                 return CreateDateTime.ToString("yyyy-MM-dd HH:mm");
            }
        
        }

    }
}
