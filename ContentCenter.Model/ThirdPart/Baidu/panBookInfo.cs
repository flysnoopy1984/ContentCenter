using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.ThirdPart.Baidu
{
    [SugarTable("panBookInfo")]
    public class panBookInfo
    {
        [SugarColumn(IsIdentity =true )]
        public long Id { get; set; }

        [SugarColumn(Length = 500)]
        public string dlink { get; set; }

        [SugarColumn(Length = 80, IsPrimaryKey = true)]
        public string fsId { get; set; }

        [SugarColumn(Length = 300)]
        public string filename { get; set; }

        [SugarColumn(Length = 300)]
        public string origFilename { get; set; }


        [SugarColumn(Length = 600)]
        public string path { get; set; }

        [SugarColumn(Length = 600)]
        public string parentPath { get; set; }

        public panConvertStatus ConvertStatus { get; set; } = panConvertStatus.CreatePanBook;

        [SugarColumn(Length = 50,IsNullable =true)]
        public string sectionCode { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string DoubanCode { get; set; }

        public long size { get; set; }

        public int isdir { get; set; }

        [SugarColumn(Length = 20)]
        public string FileType { get; set; }

        [SugarColumn(Length = 150,IsNullable =true)]
        public string GroupName { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime CreatedDateTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime ConvertDateTime { get; set; }

        [SugarColumn(IsIgnore =true)]
        public string ConvertStatueInfo
        {
            get
            {
                switch (ConvertStatus)
                {
                    case panConvertStatus.NotInDb:
                        return "未录入";
                    case panConvertStatus.ConvertToCCBook:
                        return "正式";
                    case panConvertStatus.CreatePanBook:
                        return "临时Db";
                    case panConvertStatus.FixName:
                        return "修复名字";
                    case panConvertStatus.GetDouBanRelation:
                        return "已关系";
                }
                return "";
            }
        }

        [SugarColumn(IsNullable = true)]
        //1 跑过了 0 没有跑过，-1跑了没有结果
        public bookSearchResult RunSearchResult { get; set; } = bookSearchResult.NotSearch;

        #region 静态方法
        public static string getFileType(string filename)
        {
            int pos = filename.LastIndexOf(".");
            return filename.Substring(pos + 1).ToLower();
        }

        public static string getParentRoot(string path)
        {
            int foundPos = path.LastIndexOf("/");
            string result = path.Substring(0, foundPos);
            if (foundPos == 0 && result == "")
                result = "/";
            return result;
        }

        private static List<string> _suportFileType = null;
        protected static List<string> suportFileType
        {
            get
            {
                if (_suportFileType == null)
                {
                    _suportFileType = new List<string>();
                    _suportFileType.Add("epub");
                    _suportFileType.Add("mobi");
                    _suportFileType.Add("txt");
                    _suportFileType.Add("pdf");
                    _suportFileType.Add("azw3");
                    _suportFileType.Add("docx");
                }
                return _suportFileType;

            }
        }

        public void InitBackFields(string sectionCode)
        {
            if(!string.IsNullOrEmpty(sectionCode))
                this.sectionCode = sectionCode;
            if (string.IsNullOrEmpty(this.parentPath))
                this.parentPath = panBookInfo.getParentRoot(this.path);

            this.ConvertStatus = panConvertStatus.CreatePanBook;
            this.FileType = panBookInfo.getFileType(this.filename);
            this.CreatedDateTime = DateTime.Now;
            this.origFilename = this.filename;
            
        }
        public static bool IsSupportFileType(string filetype)
        {
            return suportFileType.Contains(filetype);
          
        }

        #endregion
    }
}
