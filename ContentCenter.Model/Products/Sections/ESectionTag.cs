using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("SectionTag")]
    public class ESectionTag
    {
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public long Id { get; set; }

        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string SectionCode { get; set; }

        //[SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        //public string SectionName { get; set; }

        [SugarColumn(Length = 150, ColumnDataType = "nvarchar")]
        public string TagCode { get; set; }

        //[SugarColumn(Length = 30, ColumnDataType = "nvarchar")]
        //public string TagName { get; set; }
    }
}
