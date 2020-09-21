using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model
{
    public class VueResInfo
    {
        public string resCode { get; set; }

        private string _uploadDateTime;
        public string upLoadDateTime {
            get
            {
                return CreateDateTime.ToString("yyyy-MM-dd HH:mm");
            }
            set { _uploadDateTime = value; }
        }

        public string ownerId { get; set; }
        public string ownerName { get; set; }

        public int resType { get; set; }

        public string fileType { get; set; }

        public int goodNum { get; set; }

        public int badNum { get; set; }

        public bool IsEditing { get; set; }

        [JsonIgnore()]
        public DateTime CreateDateTime { get; set; }

        public PraizeType userPraizeType { get; set; } = PraizeType.noPraize;

        private ModelPager<VueCommentInfo> _commList;
        public ModelPager<VueCommentInfo> commList {
            get
            {
                if (_commList == null)
                    _commList = new ModelPager<VueCommentInfo>();
                return _commList;
            }
            set { _commList = value; }
        }
    }
}
