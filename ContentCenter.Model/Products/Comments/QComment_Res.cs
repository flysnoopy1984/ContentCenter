﻿using System;
using System.Collections.Generic;
using System.Text;
using IQB.Util.Models;

namespace ContentCenter.Model
{
    public class QComment_Res:QueryPager
    {
        public string resCode { get; set; }

        public string reqUserId { get; set; }

        public long fiexedCommentId { get; set; } = -1;


    }
}
