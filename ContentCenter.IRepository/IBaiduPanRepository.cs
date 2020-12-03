using ContentCenter.Model.ThirdPart.Baidu;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IRepository
{
    public interface IBaiduPanRepository : IBaseRepository<panAccessToken>
    {
        panAccessToken getAvaliableAccessToken();

        void expireAllAccessToken();
    }
}
