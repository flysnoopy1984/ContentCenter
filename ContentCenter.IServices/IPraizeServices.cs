using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IServices
{
    public interface IPraizeServices:IBaseServices<EPraize_Res>
    {
       
        long handlePraize(SubmitPraize submitPraize);

        ModelPager<VueUserPraize> queryUserPraize(QUserPraize query);
    }
}
