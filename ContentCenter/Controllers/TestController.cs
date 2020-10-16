using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using ContentCenter.AOP;
using ContentCenter.IServices;
using IQB.Util.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpPost]
       
        public ResultNormal Do()
        {
            ResultNormal r = new ResultNormal();
            r.Message = _testService.Do();
            return r;
        }
    }
}