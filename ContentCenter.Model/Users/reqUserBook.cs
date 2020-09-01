using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class reqUserBook
    {
        public string bookCode { get; set; }

        public string userId { get; set; }

        public OperationDirection direction { get; set; }
    }
}
