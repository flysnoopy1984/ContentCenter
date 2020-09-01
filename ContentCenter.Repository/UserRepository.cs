using ContentCenter.IRepository;
using ContentCenter.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.Repository
{
    public class UserRepository :BaseRepository<EUserInfo>,IUserRepository
    {
        public UserRepository(ISqlSugarClient[] sugarClient)
            : base(sugarClient.FirstOrDefault(a => a.CurrentConnectionConfig.ConfigId == CCDBConfig.MainDbKey))
        {
          
        }

        public VueUC_UserInfo getUC_User(string userId)
        {
            var q = Db.Queryable<EUserInfo>()
               .Where(u => u.Id == userId)
               .Select(u => new VueUC_UserInfo
               {
                   HeaderUrl = u.HeaderUrl,
                   Id = userId,
                   NickName = u.NickName
               });
            return q.First();
            

        }



        //public Task<EUserInfo> Login(LoginUser loginUser)
        //{
        //    return GetByKey(loginUser.LoginAccount);

        //}

        //public Task<long> RegisterNew(RegUser regUser)
        //{

        //    EUserInfo ui = new EUserInfo
        //    {
        //        UserAccount = regUser.Account,
        //        UserPwd = regUser.Pwd,
        //        Phone = regUser.Phone
        //    };
        //    return Add(ui);
        //}
    }
}
