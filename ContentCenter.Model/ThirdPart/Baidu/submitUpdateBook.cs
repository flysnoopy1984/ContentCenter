using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.Baidu
{
    //public class panUpdateBook
    //{
    //    public string fsId { get; set; }
    //    public string filename { get; set; }
    //}

    public class submitUpdateBook
    {
        public panUpdateType panUpdateType { get; set; }

        public List<panBookInfo> updateBookList { get; set; }
    }
}
