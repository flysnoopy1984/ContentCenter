using ContentCenter.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Services
{
    public class TestService : ITestService
    {
        public string Do()
        {
            return "Get TestService";
        }
    }
}
