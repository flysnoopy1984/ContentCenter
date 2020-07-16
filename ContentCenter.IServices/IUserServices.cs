using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IServices
{
    public interface IUserServices:IBaseServices<EUserInfo>
    {
        VueUerInfo Login(LoginUser loginUser);

        VueUerInfo Register(RegUser regUser);

        bool HasRegistPhone(string phone);

    }
}
