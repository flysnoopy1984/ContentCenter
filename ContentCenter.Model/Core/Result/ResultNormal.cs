using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class ResultNormal
    {
        public int ResultCode { get; set; } = 200;
        public bool IsSuccess { get; set; } = true;

        public string Message { get; set; }

        private string _ErrorMsg;
        public string ErrorMsg
        {
            get
            {
                return _ErrorMsg;
            }
            set
            {
                _ErrorMsg = value;
                IsSuccess = false;
                ResultCode = 500;
            }
        }
    }
}
