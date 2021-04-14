using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.BaseEnum
{
    public enum SectionType
    {
        Book = 0, //书本和Section直接对应
        FixTop =1, //头部3个栏目
        Column = 2, //书本和Section对应后再找到对应的Tag
        
        All = 99,
    }
    public enum TagType
    {
        book = 0,
    }

    /// <summary>
    ///网站主Section.
    /// </summary>
    public class WebSectionCode
    {

        public const string NewExpress = "NewExpress";
        public const string Popular = "Popular";
        public const string HighScore = "HighScore";
        public const string ResDownLoad = "ResDownLoad";


    }
}
