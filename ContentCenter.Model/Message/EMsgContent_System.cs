using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    /// <summary>
    /// 系统通知
    /// </summary>
    [SugarTable("ccMsgContent_System")]
    public class EMsgContent_System
    {
        public EMsgContent_System()
        {
            CreateDateTime = DateTime.Now;
        }
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnDataType= "nvarchar(max)")]
        public string htmlContent { get; set; }

        [SugarColumn(ColumnDataType = "nvarchar(200)")]
        public string htmlTitle { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
