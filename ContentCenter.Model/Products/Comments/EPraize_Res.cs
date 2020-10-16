using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model 
{
    [SugarTable("ccPraize_Res")]
    public class EPraize_Res: EPraizeDetail
    {
       // public EPraize_Res() : base(PraizeTarget.Resource) { }

        [SugarColumn(Length = 50)]
        public string ResCode { get; set; }
        
        ///// <summary>
        ///// ResCode属于哪本书，用于筛选数据 
        ///// </summary>
        //[SugarColumn(Length = 50,IsNullable =true)]
        //public string RefCode { get; set; }
       
    }
}
