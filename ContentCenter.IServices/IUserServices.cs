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
        VueUserLogin Login(LoginUser loginUser);

        VueUserLogin Register(RegUser regUser);

        bool HasRegistPhone(string phone);

        long AddFavBook(string bookCode, string userId);
        bool DelFavBook(string bookCode,string userId);

        ModelPager<VueUserBook> queryUserbookList(QUserBook query);

        VueUC_UserInfo getUC_User(string userId);

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="headerUrl"></param>
        /// <returns></returns>
        void updateHeader(string userId,string headerUrl);

        void updateInfo(VueSubmitUserInfo submitData);
    }
}
