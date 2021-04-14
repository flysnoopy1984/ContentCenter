using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
   public  class RBookNextPrev
    {
        public string CurCode { get; set; }

        public BookSimple nextBook { get; set; }

        public BookSimple prevBook { get; set; }
    }
}
