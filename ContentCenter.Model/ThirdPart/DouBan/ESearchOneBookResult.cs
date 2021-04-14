using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.DouBan
{
    [SugarTable("douban_SearchResult")]
    public class ESearchOneBookResult
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 200)]
        public string keyWord { get; set; }

        [SugarColumn(Length = 50)]
        public string Code { get; set; }

        [SugarColumn(Length = 200, ColumnDataType = "nvarchar")]
        public string Name { get; set; }

        [SugarColumn(Length = 255, IsNullable = true)]
        public string CoverUrl { get; set; }

        [SugarColumn(Length = 80, IsNullable = true)]
        public string fsId { get; set; }


    }
}
