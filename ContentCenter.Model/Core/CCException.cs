using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class CCException:Exception
    {
        public CCException(string msg) : base(msg)
        {

        }
    }
}
