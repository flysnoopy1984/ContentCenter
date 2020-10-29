using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class ResSimple
    {
        public string Code { get; set; }

        public string ShowName {
            get
            {
                return ResType == ResType.BookOss ? $"[文件]-{ResName}" : $"[URL]-{ResName}";
            }
        }

        public ResType ResType { get; set; }

        public string ResName { get; set; }
    }
}
