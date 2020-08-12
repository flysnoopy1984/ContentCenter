using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public abstract class EPraizeDetail
    {
        //public EPraizeDetail(PraizeTarget praizeTarget)
        //{
        //    PraizeTarget = praizeTarget;
        //}
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(Length = 32)]
        public string userId { get; set; }

       // public PraizeTarget PraizeTarget { get; set; }
        public PraizeType PraizeType { get; set; }
        public DateTime praizeDate { get; set; }
    }
}
