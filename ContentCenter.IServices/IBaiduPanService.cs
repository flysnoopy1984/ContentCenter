using ContentCenter.Model.BaseEnum;
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
        RpanAccessToken getAvaliableAccessToken();

        void SaveToTempDb(submitSaveBooks submitData);

        void updateTempFile(submitUpdateBook submitUpdate);
      
        List<RpanBookInfo> queryPanBookByPath(panQueryFile query);

        panFileList requireFileList(string rootPath, string accessToken);

        panFilemetaList requireFileMateList(string rootPath, string accessToken, List<string> fsIds);
    }
}
