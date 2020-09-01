using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccResourceRequire_Log")]
    public class EResourceRequire_Log
    {
        [SugarColumn(IsPrimaryKey =true, IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 50)]
        public string resCode { get; set; }

        [SugarColumn(Length = 32)]
        public string requireUserId { get; set; }

        public DateTime requireDateTime { get; set; }
    }
}
