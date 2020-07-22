
using ContentCenter.Model.Commons;
using IQB.Util.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Controllers
{
    public class CCBaseController: ControllerBase
    {
       
        protected string getUserAccount()
        {
            return getUserInfo(CConstants.Id4Claim_UserAccount);
        }
        protected string getUserId()
        {
            return getUserInfo(CConstants.Id4Claim_UserId);
        }
        protected string getUserInfo(string claimType)
        {
            var u = this.User;
            var c = u.Claims.Where(a => a.Type == claimType).FirstOrDefault();
            return c == null?null:c.Value;

        }
        protected ResultEntity<T> GetResponse<T>(T result) where T:class
        {
            ResultEntity<T> response = new ResultEntity<T>();
            response.Entity = result;
            return response;
        }

        protected ResultNormal GetError(string errorMsg)
        {
            ResultNormal response = new ResultNormal();
            response.ErrorMsg = errorMsg;
            return response;
        }
    }
}
