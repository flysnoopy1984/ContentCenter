using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public abstract class BaseMsgContent
    {
        [SugarColumn(IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 200)]
        public string OrigContent { get; set; }

        [SugarColumn(Length = 50)]
        public string BookCode { get; set; }

        [SugarColumn(Length = 100)]
        public string BookName { get; set; }

        [SugarColumn(Length = 255)]
        public string BookUrl { get; set; }

    }
}
