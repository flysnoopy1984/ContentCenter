using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.Baidu
{
    [SugarTable("panAccessToken")]
    public class panAccessToken
    {
        [SugarColumn(IsIdentity = true,IsPrimaryKey =true)]
        public int Id { get; set; }

        [SugarColumn(Length = 150)]
        public string access_token { get; set; }

        [SugarColumn(Length = 20)]
        public string expires_in { get; set; }

        [SugarColumn(Length = 150)]
        public string refresh_token { get; set; }

        public DateTime createDateTime { get; set; }

        public bool IsExpired { get; set; }
       
    }
}
