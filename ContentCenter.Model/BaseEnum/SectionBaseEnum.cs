using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Model.BaseEnum
{
    public enum SectionType
    {
        Book = 0,
        Column = 2,
        All = 99,
    }
    public enum TagType
    {
        book = 0,
    }

    /// <summary>
    ///网站主Section.
    /// </summary>
    public class WebSection
    {

        public const string NewExpress = "NewExpress";
        public const string Popular = "Popular";
        public const string HighScore = "HighScore";


    }
}
