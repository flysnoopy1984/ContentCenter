using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccPraizeInfo")]
    public class EPraizeInfo
    {
       [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

     //  [SugarColumn(Length = 50)]
       public long refId { get; set; }

       public PraizeTarget PraizeTarget { get; set; }

       public int good { get; set; }
       public int bad { get; set; }

    }
}
