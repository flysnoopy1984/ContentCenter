using ContentCenter.IRepository;
using ContentCenter.Model.ThirdPart.Baidu;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCenter.Repository
{
    public class BaiduPanRepository: BaseRepository<panAccessToken>, IBaiduPanRepository
    {
        public BaiduPanRepository(ISqlSugarClient[] sugarClient)
         : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public void expireAllAccessToken()
        {
            Db.Deleteable<panAccessToken>().Where(a => a.IsExpired == false).ExecuteCommand();
        }

        public panAccessToken getAvaliableAccessToken()
        {
            panAccessToken panAccessToken = Db.Queryable<panAccessToken>()
                .Where(a => a.IsExpired == false)
                .First();
      
            return panAccessToken;
        }
    }
}
