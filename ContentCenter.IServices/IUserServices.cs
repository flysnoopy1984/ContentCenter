using ContentCenter.Model.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IServices
{
    public interface IUserServices:IBaseServices<EUserInfo>
    {
        Task<EUserInfo> Login(string userAccount, string pwd);

    }
}
