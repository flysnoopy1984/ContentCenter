using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class RMsgContent_System: EMsgContent_System
    {
        public string dateTime {
            get
            {
                return this.CreateDateTime.ToString("yyyy-MM-dd hh:mm");
            }
        }
    }
}
