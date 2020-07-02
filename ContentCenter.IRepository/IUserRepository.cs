using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IUserRepository:IBaseRepository<EUserInfo>
    {
       // Task<EUserInfo> Login(LoginUser loginUser);

        //Task<EUserInfo> Login(LoginUser loginUser);
    }
}
