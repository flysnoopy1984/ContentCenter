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
        /// <param name="notificationType"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        int UpdateNotificationNum(NotificationType notificationType, string userId, int num=1);
    }
}
