using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{

    [SugarTable("ccUserBook")]
    public class EUserBook
    {
        [SugarColumn(IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 50, IsPrimaryKey = true)]
        public string bookCode { get; set; }

        [SugarColumn(Length =32, IsPrimaryKey = true)]
        public string userId { get; set; }

        public DateTime createdDateTime { get; set; }
    }
}
