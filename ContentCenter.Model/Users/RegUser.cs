using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class RegUser
    {
        public string Account { get; set; }
        public string Pwd { get; set; }

        public string Phone { get; set; }

        public string VerifyCode { get; set; }
    }
}
