using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model;
using IQB.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PraizeController : CCBaseController
    {
        private IPraizeServices _praizeServices;
        public PraizeController(IPraizeServices praizeServices)
        {
            _praizeServices = praizeServices;   
        }

        [HttpPost]
        public ResultNormal submit(SubmitPraize submitPraize)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                submitPraize.userId = this.getUserId();
                result.ResultId = _praizeServices.handlePraize(submitPraize);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultPager<VueUserPraize> UserPraize(QUserPraize query)
        {
            ResultPager<VueUserPraize> result = new ResultPager<VueUserPraize>();
            try
            {

                result.PageData = _praizeServices.queryUserPraize(query);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }


    }
}