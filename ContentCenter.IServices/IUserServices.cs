using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IServices
{
    public interface IUserServices:IBaseServices<EUserInfo>
    {
        EUserInfo Login(LoginUser loginUser);

        long Register(RegUser regUser);

    }
}
