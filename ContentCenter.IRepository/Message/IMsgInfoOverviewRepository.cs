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
        ///hasWelComeMsg 新用户注册包含欢迎公告
        void InitForNewUser_Sync(string userId, bool hasWelComeMsg = true);

        /// <summary>
        /// 根据类型更新通知数量
        /// </summary>
        int UpdateNotificateToUnRead(NotificationType notificationType, string userId,int num=1);

        /// <summary>
        /// 更具通知组更新所有用户的通知消息
        /// </summary>
        int UpdateGroupToUnRead(Group_Notification group, int num = 1);

        int UpdateNotificateToRead(NotificationType notificationType,string userId, int num = 1);
        /// <summary>
        /// 用户消息概况
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        VueMsgInfoOverview GetByUserId(string userId);

       
    }
}
