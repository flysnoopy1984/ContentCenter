﻿using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    [SugarTable("SectionInfo")]
    public class ESection: BaseMasterTable
    {
        public ESection()
        {
           
        }
        [SugarColumn(IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Code 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, Length = 50, ColumnDataType = "nvarchar")]
        public string Code { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        [SugarColumn(Length = 50, ColumnDataType = "nvarchar")]
        public string Title { get; set; }

        /// <summary>
        /// 栏目分网站主栏目，分栏目
        /// </summary>   
        public SectionType SectionType { get; set; }

        /// <summary>
        /// 界面排序
        /// </summary>
        [SugarColumn(DefaultValue ="0",IsNullable =true)]
        public int orderBySeq { get; set; }

       
        //[SugarColumn(DefaultValue = "0", IsNullable = true)]
        //public DataSource DataSource { get; set; }

    }
}
