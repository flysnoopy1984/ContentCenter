
using ContentCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.IRepository
{
    public interface IUserFinanceOverViewRepository: IBaseRepository<EUserFinanceOverview>
    {
        /// <summary>
        /// 更新充值积分
        /// </summary>
        /// <param name="updateFinOverview"></param>
        /// <returns></returns>
        bool updateChargePoint(updateFinOverview updateFinOverview);

        /// <summary>
        /// 更新充值金额
        /// </summary>
        /// <param name="updateFinOverview"></param>
        /// <returns></returns>
        bool updateChargeMoney(updateFinOverview updateFinOverview);

        /// <summary>
        /// 更新固定积分
        /// </summary>
        /// <param name="updateFinOverview"></param>
        /// <returns></returns>
        bool updateFixPoint(updateFinOverview updateFinOverview);

        bool updateCommission(updateFinOverview updateFinOverview);
    
        Task<VueUserFinanceOverview> getOverview(string userId);

    }
}
