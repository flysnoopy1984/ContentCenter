using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class RegUser
    {
        [JsonProperty("account")]
        public string Account { get; set; }
        
        [JsonProperty("pwd")]
        public string Pwd { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("smsVC")]
        public string VerifyCode { get; set; }
    }
}
