using ContentCenter.Model.BaseEnum;
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

        public panConvertStatus panConvertStatue { get; set; }

        //public string getFileType()
        //{
        //    int pos = server_filename.LastIndexOf(".");
        //    return server_filename.Substring(pos+1);
        //}
        public panBookInfo toPanBook(string parentPath = null)
        {
            panBookInfo panBookInfo = new panBookInfo
            {
                fsId = this.fs_id,
                filename = this.server_filename,
                origFilename = this.server_filename,
                path = this.path,
                ConvertStatus = panConvertStatus.NotInDb,
                size = this.size,
                isdir = this.isdir,
                FileType = panBookInfo.getFileType(this.server_filename),
            };
            if (!string.IsNullOrEmpty(parentPath)) panBookInfo.parentPath = parentPath;
            return panBookInfo;
        }
    }
}
