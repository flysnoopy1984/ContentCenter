using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class SubmitPraize
    {
        public PraizeTarget praizeTarget { get; set; }

        /// <summary>
        /// refCode如果是资源，则parent是BookCode
        /// </summary>
        public string parentRefCode { get; set; }
        
        public string refCode { get; set; }

        public string userId { get; set; }

        public OperationDirection praizeDirection { get; set; } 
        public PraizeType praizeType { get; set; }
    }
}
