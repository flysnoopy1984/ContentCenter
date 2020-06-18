using ContentCenter.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Controllers
{
    public class CCBaseController: ControllerBase
    {
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
