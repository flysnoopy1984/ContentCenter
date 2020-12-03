using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IUserRepository:IBaseRepository<EUserInfo>
    {
        VueUC_UserInfo getUC_User(string userId);

        Task<bool> updateHeader(string userId, string headerUrl);

        Task<bool> updateInfo(VueSubmitUserInfo submitData);

        List<UserSimple> queryNotificationGroup(Group_Notification group_Notification);

    }
}
