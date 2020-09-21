
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueSubmitUserCharge
    {
        public string userId { get; set; }

        public decimal amount { get; set; }

        public int rate { get; set; }

        public int points { get; set; }


        public DateTime pointEffectDate { get; set; }


    }
}
