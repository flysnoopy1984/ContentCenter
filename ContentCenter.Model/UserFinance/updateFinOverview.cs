using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class updateFinOverview
    {
        public string userId { get; set; }

        public OperationDirection direction { get; set; }

        public DateTime pointEffectDate { get; set; }

        public int point { get; set; } = 0;
        public int fixPoint { get; set; } = 0;
        public decimal money { get; set; } = 0;

        public decimal commission { get; set; } = 0;


    }
}
