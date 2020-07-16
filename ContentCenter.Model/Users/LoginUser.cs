using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class LoginUser
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("pwd")]
        public string Pwd { get; set; }

      
    }
}
