using ContentCenter.Common;
using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContentCenter.Services
{
 
    public class UserServices : BaseServices<EUserInfo>,IUserServices
    {
        private IUserRepository _userDb;
        private IUserBookRepository _userBookRepository;
        public UserServices(IUserRepository userRepository, IUserBookRepository userBookRepository)
            :base(userRepository)
        {
            _userBookRepository = userBookRepository;
            _userDb = userRepository;
        }

        /* 用户书本收藏夹 Begin */
        public long AddFavBook(string bookCode,string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("非法操作！");
            return _userBookRepository.AddUserBook(bookCode, userId).Result;
        }

        public bool DelFavBook(string bookCode, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("非法操作！");
            return _userBookRepository.DelUserBook(bookCode, userId).Result;
        }

        public ModelPager<VueUserBook> queryUserbookList(QUserBook query)
        {
            if (string.IsNullOrEmpty(query.userId))
                throw new Exception("非法操作！");
            return _userBookRepository.queryUserBook(query).Result;
        }

        /* 用户书本收藏夹 End */

        public bool HasRegistPhone(string phone)
        {
             var r = _userDb.GetCount(a => a.Phone == phone).Result;
            return r > 0;
        }

        public VueUerInfo Login(LoginUser loginUser)
        {
            EUserInfo result = null;
            result = _userDb.GetByExpSingle(a=>a.UserAccount == loginUser.Account && a.UserPwd == loginUser.Pwd).Result;
            if (result == null)
                throw new CCException(CCWebMsg.User_Login_WrongUserPwd);
            return result.ToVueUser();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="regUser"></param>
        /// <returns>-1 已存在,-2手机已使用</returns>
        public VueUerInfo Register(RegUser regUser)
        {
            int c = _userDb.GetCount(a=>a.UserAccount == regUser.Account).Result;
            int phone = _userDb.GetCount(a => a.Phone == regUser.Phone).Result;
            if (c > 0)
                throw new CCException(CCWebMsg.User_Reg_Exist_Account);
            if (phone > 0)
                throw new CCException(CCWebMsg.User_Reg_Exist_Phone);

            EUserInfo ui = new EUserInfo
            {
                Id = CodeManager.UserCode(),  //Guid.NewGuid().ToString("N"),
                UserAccount = regUser.Account,
                UserPwd = regUser.Pwd,
                Phone = regUser.Phone,
                NickName = regUser.Account,
            };
            var recOp = _userDb.AddNoIdentity(ui).Result;
            return ui.ToVueUser();

        }

        public VueUC_UserInfo getUC_User(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("非法操作！");
            return _userDb.getUC_User(userId);
        }

        public void updateHeader(string userId, string headerUrl)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("非法操作！");
            _userDb.updateHeader(userId, headerUrl);
        }

        public void updateInfo(VueSubmitUserInfo submitData)
        {
            if (string.IsNullOrEmpty(submitData.userId))
                throw new Exception("非法操作！");
            _userDb.updateInfo(submitData);
           // _userDb.UpdatePart_NoObj()
        }

    }
}
