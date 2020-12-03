using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model.ThirdPart.Baidu;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Services
{
    public class BaiduPanService :  BaseServices<panAccessToken>, IBaiduPanService
    {
        private IBaiduPanRepository _baiduPanRepository;
        public BaiduPanService(
           IBaiduPanRepository baiduPanRepository)
         : base(baiduPanRepository)
        {
            _baiduPanRepository = baiduPanRepository;
         
        }

        public panAccessToken getAvaliableAccessToken()
        {
            return _baiduPanRepository.getAvaliableAccessToken();
        }

        public int SaveAccessToken(panAccessToken panAccessToken)
        {
            if (panAccessToken.Id <= 0)
            {
                panAccessToken.createDateTime = DateTime.Now;
                _baiduPanRepository.expireAllAccessToken();
                return _baiduPanRepository.AddNoIdentity_Sync(panAccessToken);
            }
            return 0;
           
        }
    }
}
