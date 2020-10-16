using ContentCenter.IRepository;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class MsgPraizeRepository : BaseRepository<EMsgInfo_Praize>, IMsgPraizeRepository
    {
        public MsgPraizeRepository(ISqlSugarClient[] sugarClient)
         : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }
        #region Message
        public bool ExistMsgPraize_Sync(long refId, PraizeTarget praizeTarget, string sendUserId)
        {
            var r = base.GetCount(
                c => c.RefId == refId &&
                c.PraizeTarget == praizeTarget &&
                c.SendUserId == sendUserId 
             //   c.NotificationStatus == NotificationStatus.
                ).Result;

            return r > 0;
        }
       
        #endregion

        #region Content

        public EMsgContent_Praize GetContentPraize_Sync(long refId, PraizeTarget praizeTarget)
        {
            return Db.Queryable<EMsgContent_Praize>().Where(
                c => c.RefId == refId &&
              c.PraizeTarget == praizeTarget
              ).First();

        }

       
        public long AddContentPraize_Sync(EMsgContent_Praize content)
        {
            var insertable = Db.Insertable<EMsgContent_Praize>(content);
            return insertable.ExecuteReturnBigIdentity();
        }

       

        #endregion

        public async Task<ModelPager<VueMsgInfo_Praize>> msgPraizeList(QUserMsg query)
        {
            ModelPager<VueMsgInfo_Praize> result = new ModelPager<VueMsgInfo_Praize>(query.pageIndex, query.pageSize);


            return result;
        }

      
    }
}
