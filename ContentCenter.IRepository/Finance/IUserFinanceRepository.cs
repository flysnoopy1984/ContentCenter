using ContentCenter.Model;
using IQB.Util.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IUserFinanceRepository: IBaseRepository<EUserChargeTrans>
    {
        Task<ModelPager<VueUserChargeTrans>> getChargeTrans(QUserTrans query);

        Task<ModelPager<VueUserPointTrans>> getPointTrans(QUserTrans query);

        Task<ModelPager<VueUserCommissionTrans>> getCommissionTrans(QUserTrans query);

        Task<ModelPager<VueUserBalanceTrans>> getBalanceTrans(QUserTrans query);

        bool AddPointTrans_Sync(EUserPointsTrans newEntity);
    }
}
