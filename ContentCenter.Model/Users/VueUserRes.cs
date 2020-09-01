using ContentCenter.Model.BaseEnum;
using ContentCenter.Model.Products.Res;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueUserRes
    {
        public string bookCode { get; set; }
        public string bookName { get; set; }

        public string bookCoverUrl { get; set; }
        /// <summary>
        /// 用户前端
        /// </summary>
        public bool IsEdit { get; set; } = false;

        private List<VueBookRes> _resList;
        public List<VueBookRes> resList
        {
            get
            {
                if (_resList == null) _resList = new List<VueBookRes>();
                return _resList;
            }
            set { _resList = value; }
        }

    }
}
