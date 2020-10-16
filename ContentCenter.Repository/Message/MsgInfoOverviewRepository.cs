using ContentCenter.IRepository;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ContentCenter.Repository
{
    public class MsgInfoOverviewRepository: BaseRepository<EMsgInfoOverview>, IMsgInfoOverviewRepository
    {
        public MsgInfoOverviewRepository(ISqlSugarClient[] sugarClient)
         : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {

        }

        public void InitForNewUser_Sync(string userId)
        {
            EMsgInfoOverview msgInfoOverview = new EMsgInfoOverview()
            {
                userId = userId,
            };
            base.AddNoIdentity_Sync(msgInfoOverview);
        }

        /// <summary>
        /// 1更新某种类型通知数
        /// 2 更新通知总数
        /// </summary>
        /// <param name="notificationType"></param>
        /// <param name="userId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int UpdateNotificationNum(NotificationType notificationType, string userId, int num = 1)
        {
            var op = Db.Updateable<EMsgInfoOverview>()
                .SetColumnsIF(notificationType == NotificationType.praize,
                m=>new EMsgInfoOverview { nPraize = m.nPraize+num, notificationTotal = m.notificationTotal + num })

                .SetColumnsIF(notificationType == NotificationType.comment,
                m => new EMsgInfoOverview { nComment = m.nComment + num, notificationTotal = m.notificationTotal + num })

                .SetColumnsIF(notificationType == NotificationType.reply,
                 m => new EMsgInfoOverview { nReply = m.nReply + num, notificationTotal = m.notificationTotal + num })

                .SetColumnsIF(notificationType == NotificationType.message,
                  m => new EMsgInfoOverview { messageTotal = m.messageTotal + num })
                .Where(m=>m.userId == userId);

            return op.ExecuteCommand();
        }
    }
}
