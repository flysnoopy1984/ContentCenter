using ContentCenter.Model.ThirdPart.Baidu;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IServices
{
    public interface IBaiduPanService : IBaseServices<panAccessToken>
    {
      
        /// 写入AccessToken
        int SaveAccessToken(panAccessToken panAccessToken);

        /// <summary>
        ///  如果已过期，返回null
        /// </summary>
        panAccessToken getAvaliableAccessToken();
    }
}
