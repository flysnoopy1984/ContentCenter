using ContentCenter.IRepository;
using ContentCenter.IServices;
using ContentCenter.Model.Users;
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
        public async Task<EUserInfo> Login(string userAccount, string pwd)
        {
         //   Thread.Sleep(1000 * 5);
            var result = await _userDb.GetByKey(4);
            return result;
        }
    }
}
