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
      

        public EUserInfo Login(LoginUser loginUser)
        {
            EUserInfo result = null;
            result = _userDb.GetByExpSingle(a=>a.UserAccount == loginUser.Account && a.UserPwd == loginUser.Pwd).Result;
            if (result == null)
                throw new CCException(CCWebMsg.User_Login_WrongUserPwd);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regUser"></param>
        /// <returns>-1 已存在</returns>
        public long Register(RegUser regUser)
        {
            int c = _userDb.GetCount(regUser.Account).Result;
            if (c > 0) return -1;
            EUserInfo ui = new EUserInfo
            {
                Id = Guid.NewGuid().ToString("N"),
                UserAccount = regUser.Account,
                UserPwd = regUser.Pwd,
                Phone = regUser.Phone,
                NickName = regUser.Account,
            };
            return _userDb.AddNoIdentity(ui).Result;
           
            
           
        }
    }
}
