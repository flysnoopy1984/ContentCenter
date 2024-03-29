﻿using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("BookInfo")]
    public class EBookInfo: BaseMasterTable
    {
       [SugarColumn(IsIdentity =true)]
        public long Id { get; set; }

        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDataType = "varchar")]
        public string Code { get; set; }

        [SugarColumn(Length = 100)]
        public string Title { get; set; }

        /// <summary>
        /// 原作书名
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
        public string OrigTitle { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
        public string SubTitle { get; set; }

        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string AuthorCode { get; set; }

        /// <summary>
        /// 译者
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Translater { get; set; }


        /// <summary>
        /// 出版社
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Publisher { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = true, ColumnDataType = "nvarchar")]
        public string PublishDate { get; set; }

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

        [SugarColumn(Length = 255, IsNullable = true)]
        public string CoverUrl_Big { get; set; }
        /// <summary>
        /// 装帧
        /// </summary>
        [SugarColumn(Length = 30, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Makeup { get; set; }

        /// <summary>
        /// 丛书
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Series { get; set; }

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

        public Double Score { get; set; } = 0;

        [SugarColumn(Length = 20, IsNullable = true)]
        public string ISBN { get; set; }

        [SugarColumn(Length = 255, IsNullable = true)]
        public string TranslaterUrl { get; set; }

        /// <summary>
        /// 抓爬网站BookId
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = true)]
        public string SourceBookId { get; set; }

        /// <summary>
        /// 出品方
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Producer { get; set; }

        [SugarColumn(Length = 255, IsNullable = true)]
        public string ProducerUrl { get; set; }

        [SugarColumn(Length = 255, IsNullable = true)]
        public string SeriesUrl { get; set; }

        /// <summary>
        /// 是否虚构
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public FictionType FictionType { get; set; }

        [SugarColumn(IsNullable = true)]
        public DataSource DataSource { get; set; }

        [SugarColumn(IsNullable = true,DefaultValue ="0")]
        public int ResoureNum { get; set; }

    }
}
