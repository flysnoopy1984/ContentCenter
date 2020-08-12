using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccSearchKeyLog")]
    public class ESearchKeyLog:BaseMasterTable
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 32)]
        public string UserId { get; set; }

        [SugarColumn(Length = 100,ColumnDataType ="nvarchar")]
        public string searchKey { get; set; }

    }
}
