using ContentCenter.IRepository;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCenter.Repository
{
    public class MsgCommentResRepository : BaseRepository<EMsgInfo_CommentRes>, IMsgCommentResRepository
    {
        public MsgCommentResRepository(ISqlSugarClient[] sugarClient)
          : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        #region Content
        public long AddContentCommentRes_Sync(EMsgContent_CommentRes content)
        {
            var insertable = Db.Insertable(content);
            return insertable.ExecuteReturnBigIdentity();
        }

        public EMsgContent_CommentRes GetContentCommentRes_Sync(string recCode)
        {
            return Db.Queryable<EMsgContent_CommentRes>().Where(
               c => c.ResCode == recCode)
             .First();
        }

        #endregion

        #region Msg

        //public bool ExistMsgCommentRes_Sync(string resCode, string sendUserId)
        //{
        //    var r = base.GetCount(c => c.resCode == resCode && c.SendUserId == sendUserId).Result;

        //    return r > 0;
        //}

       
        #endregion
    }
}
