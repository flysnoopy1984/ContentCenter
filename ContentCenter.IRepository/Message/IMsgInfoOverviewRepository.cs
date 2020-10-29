using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IRepository
{
    public interface IMsgInfoOverviewRepository : IBaseRepository<EMsgInfoOverview>
    {
        /// <summary>
        /// 初始化用户
        /// </summary>
        /// <param name="userId"></param>
        void InitForNewUser_Sync(string userId);

        /// <summary>
        /// 根据类型更新通知数量
        /// </summary>
        int UpdateNotificateToUnRead(NotificationType notificationType, string userId,int num=1);

        int UpdateNotificateToRead(NotificationType notificationType,string userId, int num = 1);
        /// <summary>
        /// 用户消息概况
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        VueMsgInfoOverview GetByUserId(string userId);

       
    }
}
