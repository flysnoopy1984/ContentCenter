using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("DataSection")]
    public  class ESectionBook
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string SectionCode { get; set; }

        [SugarColumn(ColumnName = "ItemCode", Length = 50, ColumnDataType = "nvarchar")]
        public string BookCode { get; set; }

        [SugarColumn(Length = 40, IsNullable = true)]
        public string BatchNo { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
