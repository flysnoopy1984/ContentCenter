using IQB.Util;
using System;

namespace BookTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            bool result = VerifyUtil.VerifyHttp("http://iqianba-public.oss-cn-shanghai.aliyuncs.com/Books/DB_33503403/Epub/c829a3fa78574e758c0e7e6d9b592f8e_%E5%B9%B3%E5%87%A1%E7%9A%84%E4%B8%96%E7%95%8C%EF%BC%88%E5%85%A8%E4%B8%89%E9%83%A8%EF%BC%89.Epub");

            Console.WriteLine($"VerifyUrl:{result}");
        }
    }
}
