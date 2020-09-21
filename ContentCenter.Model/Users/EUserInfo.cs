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

        [SugarColumn(Length = 256, IsNullable = true)]
        public string HeaderUrl { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string Phone { get; set; }

        public int Age { get; set; }

        public int Sex { get; set; }

        [SugarColumn(Length = 32, IsNullable = true)]
        public string wxOpenId { get; set; }

        /// <summary>
        /// 0-10 普通用户 10-100 会员 100-200 vip 200-300 svip
        /// </summary>
        [SugarColumn(DefaultValue ="0")]
        public int vipLevel { get; set; } = 0;

        public VueUerInfo ToVueUser()
        {
            return new VueUerInfo
            {
                HeaderUrl = this.HeaderUrl,
                NickName = this.NickName,
                UserAccount = this.UserAccount,
                TokenPwd = this.UserPwd,
                UserId = this.Id,
                Sex = this.Sex,
            };
        }


    }
}
