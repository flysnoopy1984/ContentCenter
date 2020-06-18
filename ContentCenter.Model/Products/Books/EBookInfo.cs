using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("BookInfo")]
    public class EBookInfo: BaseProduct
    {
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDataType = "nvarchar")]
        public string Code { get; set; }

        [SugarColumn(Length = 100)]
        public string Title { get; set; }

        [SugarColumn(Length = 50)]
        public string AuthorCode { get; set; }


        /// <summary>
        /// 出版社
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Publisher { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = true)]
        public string PageCount { get; set; }

        /// <summary>
        /// 定价
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = true)]
        public string Pricing { get; set; }

        [SugarColumn(Length = 255, IsNullable = true)]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 目录
        /// </summary>
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Catalog { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Summery { get; set; }

        public Double Score { get; set; }

    }
}
