using ContentCenter.IRepository;
using ContentCenter.Model;
using ContentCenter.Model.BaseEnum;
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
                   UserId = userId,
                   NickName = u.NickName,
                   Sex = u.Sex
                   
                   
               });
            return q.First();
            

        }

        public List<UserSimple> queryNotificationGroup(Group_Notification group_Notification)
        {
            return Db.Queryable<EUserInfo>().Where(u => u.Group_Notification == group_Notification)
                .Select(u => new UserSimple
                {
                    UserId = u.Id
                }).ToList();
        }

        public Task<bool> updateHeader(string userId, string headerUrl)
        {
            return base.UpdatePart_NoObj(a => new EUserInfo { HeaderUrl = headerUrl },a => a.Id == userId);
        }

        public Task<bool> updateInfo(VueSubmitUserInfo submitData)
        {
            return base.UpdatePart_NoObj(a => 
                 new EUserInfo 
                 { 
                     NickName = submitData.nickName,
                     Sex = Convert.ToInt32(submitData.sex) 
                 },
                a => a.Id == submitData.userId);
        }



       
    }
}
