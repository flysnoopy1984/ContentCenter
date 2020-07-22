using ContentCenter.Common;
using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model;
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
        public UserServices(IUserRepository userRepository)
            :base(userRepository)
        {
            _userDb = userRepository;
        }

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
    }
}
