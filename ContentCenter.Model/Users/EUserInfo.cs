using ContentCenter.Model.BaseModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.Users
{
    [SugarTable("ccUserInfo")]
    public class EUserInfo: BaseMasterTable
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(Length = 40)]
        public string NickName { get; set; }

        [SugarColumn(Length = 20)]
        public string RealName { get; set; }

        [SugarColumn(Length = 32)]
        public string wxOpenId { get; set; }

        [SugarColumn(Length = 100)]
        public string wxName { get; set; }

        [SugarColumn(Length = 20)]
        public string wxCity { get; set; }

        [SugarColumn(Length = 20)]
        public string wxProvince { get; set; }

        [SugarColumn(Length = 20)]
        public string wxCountry { get; set; }

        [SugarColumn(Length = 256)]
        public string HeaderUrl { get; set; }

        [SugarColumn(Length = 20)]
        public string Phone { get; set; }

        public int Age { get; set; }

        public int Sex { get; set; }


    }
}
