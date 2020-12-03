using BookTasks.setup;
using IQB.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.IO;
using Tools.BookReader;

namespace BookTasks
{
    class Program
    {
        private static readonly IConfigurationBuilder Configuration = new ConfigurationBuilder();
        private static IConfiguration _configuration;
        private static ServiceProvider _ServiceProvider;

        static void Main(string[] args)
        {
            //   bool result = VerifyUtil.VerifyHttp("http://iqianba-public.oss-cn-shanghai.aliyuncs.com/Books/DB_33503403/Epub/c829a3fa78574e758c0e7e6d9b592f8e_%E5%B9%B3%E5%87%A1%E7%9A%84%E4%B8%96%E7%95%8C%EF%BC%88%E5%85%A8%E4%B8%89%E9%83%A8%EF%BC%89.Epub");

            //   Console.WriteLine($"VerifyUrl:{result}");
            try
            {
                //   InitSystem();
                Test();
              //  ccEpubReader ccEpubReader = new ccEpubReader();
              // ccEpubReader.Analy();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
            Console.ReadLine();
         
            // _ServiceProvider.
        }

        private static void Test()
        {
            string at = "121.ce7809e541f0a6d850a66f0301b12459.YnTZ1kEcU4WMzSUogItL_PKqhtarjGpc84oVZl-.v1bOuA";
            Console.WriteLine(at.Length);
        }

        private static void InitSystem()
        {
            dynamic type = (new Program()).GetType();

            string currentDirectory = Path.GetDirectoryName(type.Assembly.Location);

            _configuration = Configuration.SetBasePath(currentDirectory)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSqlSugarSetup(_configuration);

            serviceCollection.AddScoped<BookTasks>();

            _ServiceProvider = serviceCollection.BuildServiceProvider();
          
        }
    }
}
