using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookTasks
{
    public class BookTasks
    {
        ISqlSugarClient _db;
        public BookTasks(ISqlSugarClient db)
        {
            _db = db;

            _db.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
            {
                Console.WriteLine(sql);
              //  NLogUtil.cc_InfoTxt($"Sql:{sql}");
            };
            _db.Aop.OnError = (exp) =>//执行SQL 错误事件
            {
               // NLogUtil.cc_ErrorTxt($"Sql:{exp.Sql}");
                 Console.WriteLine(exp.Sql);
                //exp.sql exp.parameters 可以拿到参数和错误Sql             
            };


        }
        public void ChangeBookCover()
        {
           

        //    var book = _db.Queryable<EBookInfo>().First();
         //   var r = ConvertToSmallCoverUrl(book.CoverUrl);
            var r =_db.Updateable<EBookInfo>()
              .SetColumns(m => new EBookInfo
              {
                  CoverUrl = m.CoverUrl_Big.Replace("/l/", "/s/")
              })
               .Where(m=>m.Code !="")
              .ExecuteCommand();
            Console.WriteLine($"影响行数:{r}");
            Console.WriteLine("完成 ChangeBookCover");
        }
        private string ConvertToSmallCoverUrl(string bigUrl)
        {
            //https://img3.doubanio.com/view/subject/l/public/s33741751.jpg
            //https://img3.doubanio.com/view/subject/s/public/s33741751.jpg
           var s =  bigUrl.Replace("/l/","/s/");
          // var t = bigUrl.Substring

            return s;
        }
    }
}
