using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.Baidu
{
    [SugarTable("panBookInfo")]
    public class panBookInfo
    {
        [SugarColumn(IsIdentity =true ,IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 500)]
        public string dlink { get; set; }

        [SugarColumn(Length = 80)]
        public string fsId { get; set; }

        [SugarColumn(Length = 80)]
        public string filename { get; set; }

        [SugarColumn(Length = 300)]
        public string path { get; set; }

        public int level { get; set; }

        public long pId { get; set; }
    }
}
