using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IServices
{
    public interface IPraizeServices:IBaseServices<EPraize_Res>
    {
        long handlePraize(SubmitPraize submitPraize);

       
    }
}
