using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class PraizeBaseMasterTable:BaseMasterTable
    {
        [SugarColumn(DefaultValue = "0")]
        public int goodNum { get; set; } = 0;

        [SugarColumn(DefaultValue = "0")]
        public int badNum { get; set; } = 0;


    }
}
