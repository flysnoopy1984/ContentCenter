using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IServices
{
    public interface ISystemServices: IBaseServices<ESysConfig>
    {
        public ESysConfig GetSysConfig();
    }
}
