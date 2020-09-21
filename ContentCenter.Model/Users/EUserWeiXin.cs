using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccUserWeixin")]
    public class EUserWeixin
    {
        [SugarColumn(IsIdentity =true)]
        public string Id { get; set; }

        [SugarColumn(Length = 32, IsPrimaryKey = true)]
        public string OpenId { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string Name { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string City { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string Province { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string Country { get; set; }
    }
}
