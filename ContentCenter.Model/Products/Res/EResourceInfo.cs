using ContentCenter.Model.BaseEnum;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("ccResourceInfo")]
    public class EResourceInfo: BaseMasterTable
    {
        public EResourceInfo(){}
       

        [SugarColumn(IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(IsPrimaryKey = true,Length = 50, ColumnDataType = "varchar")]
        public string Code { get; set; }

        /// <summary>
        /// 资源关联的Code,如果是Book 就是书资源，
        /// </summary>
        [SugarColumn(Length = 50)]
        public string RefCode { get; set; }

        /// <summary>
        /// 资源上传者
        /// </summary>
        [SugarColumn(Length = 32)]
        public string Owner { get; set; }

        /// <summary>
        /// 资源类型BookOss BookUrl
        /// </summary>
        public ResType ResType { get; set; }

        /// <summary>
        /// 文件类型 epub pdf txt
        /// </summary>
        [SugarColumn(Length = 20)]
        public string FileType { get; set; }


        /// <summary>
        /// Oss 资源使用
        /// </summary>
        [SugarColumn(Length = 150, IsNullable = true)]
        public string OssPath { get; set; }

        /// <summary>
        /// 源文件名称
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string OrigFileName { get; set; }

        /// <summary>
        /// 外部 Url资源使用
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true)]
        public string Url { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true,ColumnDataType = "nvarchar")]
        public string Remark { get; set; }

        [SugarColumn(DefaultValue = "0")]
        public int goodNum { get; set; } = 0;

        [SugarColumn(DefaultValue = "0")]
        public int badNum { get; set; } = 0;

        /// <summary>
        /// 资源请求次数
        /// </summary>
        [SugarColumn(DefaultValue = "0")]
        public int requireNum { get; set; } = 0;
       

    }
}
