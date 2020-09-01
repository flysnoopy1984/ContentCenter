using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IUserRepository:IBaseRepository<EUserInfo>
    {
        VueUC_UserInfo getUC_User(string userId);
        // Task<EUserInfo> Login(LoginUser loginUser);

        //Task<EUserInfo> Login(LoginUser loginUser);
    }
}
