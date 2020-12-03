using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.Baidu
{
    public class panFileList_item
    {
        public string category { get; set; }
        public string fs_id { get; set; }

        public int isdir { get; set; }

        public string path { get; set; }

        public string server_filename { get; set; }

        public long size { get; set; }

    }
}
