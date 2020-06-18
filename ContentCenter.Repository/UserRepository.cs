using ContentCenter.IRepository;
using ContentCenter.Model.Users;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.Repository
{
    public class UserRepository :BaseRepository<EUserInfo>,IUserRepository
    {
        public UserRepository(ISqlSugarClient sugarClient)
            :base(sugarClient)
        {
            var a = 1;
          //  sugarClient
        }
    }
}
