using ContentCenter.Model.BaseEnum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class UploadRes
    {
        /// <summary>
        /// 外部Code
        /// </summary>
        public string refCode { get; set; }

        /// <summary>
        /// 用户账户
        /// </summary>
        public string owner { get; set; }

        /// <summary>
        /// 文件类型 epub,pdf txt mobi
        /// </summary> 
        public string fileType { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public ResType resType { get; set; }

        public string outerUrl { get; set; }

        public string remark { get; set; }

        public bool IsReset { get; set; } = false;

      

        

        

    }
}
