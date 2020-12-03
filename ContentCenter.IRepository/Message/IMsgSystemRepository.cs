using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCenter.IRepository
{
    public interface IMsgSystemRepository:IBaseRepository<EMsgInfo_System>
    {
        EMsgContent_System GetContentSystem_Sync(long Id);

        void AddContentSystem(EMsgContent_System content);

        public ModelPager<VueSystemNotification> querySystemNotification(QMsgUser query);

        int UpdateMsgStatus(SubmitUnReadMsgIdList submitData);
    }
}
