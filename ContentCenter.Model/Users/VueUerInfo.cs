using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueUerInfo
    {
        public ResponseToken Token { get; set; }

        public string UserId { get; set; }

        public string NickName { get; set; }

        public string UserAccount { get; set; }
        public string TokenPwd { get; set; }

        public string HeaderUrl { get; set; }

        public int Sex { get; set; }

        public string levelInfo { get; set; }
        public int level { get; set; }



        //public string Phone { get; set; }


    }
}
