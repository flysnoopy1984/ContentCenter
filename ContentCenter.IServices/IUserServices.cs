using ContentCenter.Model;
using IQB.Util.Models;
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

        long AddFavBook(string bookCode, string userId);
        bool DelFavBook(string bookCode,string userId);

        ModelPager<VueUserBook> queryUserbookList(QUserBook query);

        VueUC_UserInfo getUC_User(string userId);
    }
}
