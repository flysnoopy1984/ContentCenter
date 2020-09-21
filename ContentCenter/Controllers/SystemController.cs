using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model;
using ContentCenter.Model.Commons;
using IQB.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
   // [Authorize]
    public class SystemController : CCBaseController
    {
        private ISystemServices _systemServices;
      
        public SystemController(ISystemServices systemServices)
        {
            _systemServices = systemServices;
           
        }
        [HttpGet]
        public ResultEntity<ESysConfig> GetConfig()
        {
            ResultEntity<ESysConfig> result = new ResultEntity<ESysConfig>();
            try
            {
                result.Entity = _systemServices.GetSysConfig();
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
    }
}