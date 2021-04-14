using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.Baidu
{
    public class submitSaveBooks
    {
        public string accessToken { get; set; }
        //暂时不支持
        public bool searchDeep { get; set; }
        public string startPath { get; set; }

        public string sectionCode { get; set; }

      
        public submitbookType submitbookType { get; set; }

        public List<panBookInfo> panBookList { get; set; }


    }
}
