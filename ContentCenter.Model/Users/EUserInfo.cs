using ContentCenter.Model.BaseModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccUserInfo")]
    public class EUserInfo: BaseMasterTable
    {
        [SugarColumn(IsPrimaryKey = true,Length = 32)]
        public string Id { get; set; }

        [SugarColumn(Length =40,ColumnDataType = "nvarchar")]
        public string UserAccount { get; set; }

        [SugarColumn(Length = 40)]
        public string UserPwd { get; set; }

        [SugarColumn(Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
        public string NickName { get; set; }

        [SugarColumn(Length = 10, ColumnDataType = "nvarchar", IsNullable = true)]
        public string RealName { get; set; }

        [SugarColumn(Length = 32, IsNullable = true)]
        public string wxOpenId { get; set; }

        [SugarColumn(Length = 100, IsNullable = true)]
        public string wxName { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string wxCity { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string wxProvince { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string wxCountry { get; set; }

        [SugarColumn(Length = 256, IsNullable = true)]
        public string HeaderUrl { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string Phone { get; set; }

        public int Age { get; set; }

        public int Sex { get; set; }


    }
}
