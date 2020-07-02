using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public abstract class BaseMasterTable
    {
        
        public BaseMasterTable()
        {
            CreateDateTime = DateTime.Now;
            UpdateDateTime = DateTime.Now;
        }
        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public bool IsDelete { get; set; }
    }
}
