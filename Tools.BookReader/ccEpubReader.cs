using EpubSharp;
using System;

namespace Tools.BookReader
{
    public class ccEpubReader
    {
        public ccEpubReader()
        {

        }
        public void Analy()
        {
            var path = @"D:\MyBook\平凡的世界（全三部）2.Epub";
            path = @"D:\MyBook\薛兆丰经济学讲义_-_薛兆丰.epub";
            EpubBook book = EpubReader.Read(path);
            string title = book.Title;
            var authors = book.Authors;
            var cover = book.CoverImage;

            var chapters = book.TableOfContents;

            var html = book.Resources.Html;
          
        }
    }
}
