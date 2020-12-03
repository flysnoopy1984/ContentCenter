using ContentCenter.IRepository;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using IQB.Util;
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

        public VueMsgInfoOverview GetByUserId(string userId)
        {
           var  r =  Db.Queryable<EMsgInfoOverview>().Where(m => m.userId == userId)
                .Select(m => new VueMsgInfoOverview
                {
                    messageTotal = m.messageTotal,
                    nComment = m.nComment,
                    notificationTotal = m.notificationTotal,
                    nPraize = m.nPraize,
                    nReply = m.nReply,
                    nSystem = m.nSystem,
                    userId = m.userId,
                });
            return r.First();
        }

        public void InitForNewUser_Sync(string userId,bool hasWelComeMsg=true)
        {
           
            try
            {
                EMsgInfoOverview msgInfoOverview = new EMsgInfoOverview()
                {
                    userId = userId,
                };
                if (hasWelComeMsg)
                {
                    msgInfoOverview.nSystem = 1;
                    msgInfoOverview.notificationTotal = 1;
                }
                base.AddNoIdentity_Sync(msgInfoOverview);
            }
            catch (Exception ex)
            {
                NLogUtil.cc_ErrorTxt("[MsgInfoOverviewRepository]-InitForNewUser_Sync:" + ex.Message);
            }
           
        }

        /// <summary>
        /// 1 更新某种类型通知状态到已读
        /// 2 更新通知总数
        /// </summary>
        public int UpdateNotificateToRead(NotificationType notificationType, string userId, int num = 1)
        {
            if (num <= 0) return 0;

            var op = Db.Updateable<EMsgInfoOverview>()
                .SetColumnsIF(notificationType == NotificationType.praize,
                m => new EMsgInfoOverview { nPraize = m.nPraize - num,
                    notificationTotal = m.notificationTotal - num,
                    readPraize = m.readPraize + num,

                })

                .SetColumnsIF(notificationType == NotificationType.comment,
                m => new EMsgInfoOverview { 
                    nComment = m.nComment - num, 
                    notificationTotal = m.notificationTotal - num ,
                    readComment = m.readComment +num,
                })

                .SetColumnsIF(notificationType == NotificationType.reply,
                 m => new EMsgInfoOverview { nReply = m.nReply - num, 
                     notificationTotal = m.notificationTotal - num,
                     readReply = m.readReply+num,
                 })
                .SetColumnsIF(notificationType == NotificationType.system,
                 m => new EMsgInfoOverview
                 {
                     nSystem = m.nSystem - num,
                     notificationTotal = m.notificationTotal - num,
                 })

                .SetColumnsIF(notificationType == NotificationType.message,
                  m => new EMsgInfoOverview
                  {
                      messageTotal = m.messageTotal - num,

                  })
                .Where(m => m.userId == userId);

            var r = op.ExecuteCommand();

            return r;
        }

        /// <summary>
        /// 1 更新某种类型通知未读数
        /// 2 更新通知总数
        /// </summary>
        public int UpdateNotificateToUnRead(NotificationType notificationType, string userId, int num = 1)
        {
            if (num <= 0) return 0;

            var op = Db.Updateable<EMsgInfoOverview>()
                .SetColumnsIF(notificationType == NotificationType.praize,
                m => new EMsgInfoOverview { nPraize = m.nPraize + num, notificationTotal = m.notificationTotal + num })

                .SetColumnsIF(notificationType == NotificationType.comment,
                m => new EMsgInfoOverview { nComment = m.nComment + num, notificationTotal = m.notificationTotal + num })

                .SetColumnsIF(notificationType == NotificationType.reply,
                 m => new EMsgInfoOverview { nReply = m.nReply + num, notificationTotal = m.notificationTotal + num })

                 .SetColumnsIF(notificationType == NotificationType.system,
                 m => new EMsgInfoOverview { nSystem = m.nSystem + num, notificationTotal = m.notificationTotal + num })

                .SetColumnsIF(notificationType == NotificationType.message,
                  m => new EMsgInfoOverview { messageTotal = m.messageTotal + num })
                .Where(m => m.userId == userId);

            var r = op.ExecuteCommand();
          
            return r;
        }

        public int UpdateGroupToUnRead(Group_Notification group, int num = 1)
        {
            if (num <= 0) return 0;

            var op = Db.Updateable<EMsgInfoOverview>()
              .SetColumns(m => new EMsgInfoOverview { nSystem = m.nSystem + num, notificationTotal = m.notificationTotal + num })
              .Where(m => m.userId == SqlFunc.Subqueryable<EUserInfo>()
                                    .Where(u => u.Id == m.userId && u.Group_Notification == group)
                                    .Select(u => u.Id)
             );
            var r = op.ExecuteCommand();

            return r;
        }
    }
}
